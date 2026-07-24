# OFFSET Pagination vs Keyset Pagination

## Diferença entre SQL Offset Pagination e Keyset Pagination

### Offset Pagination (Traditional)

```sql
SELECT * FROM Employees ORDER BY EmployeeID LIMIT 15 OFFSET 10000;
```

- Usa `OFFSET` para "pular" linhas anteriores
- O banco lê e descarta as 10.000 primeiras linhas antes de retornar as 15 desejadas
- **Performance degrada linearmente** conforme o offset aumenta (O(n))
- Página 1 e página 1000 têm custo muito diferente
- Permite ir para qualquer página arbitrariamente

### Keyset Pagination (Cursor-based)

```sql
SELECT * FROM Employees WHERE EmployeeID > 10000 ORDER BY EmployeeID LIMIT 15;
```

- Usa um **cursor** (último ID visto) em vez de offset
- O banco usa o índice diretamente para localizar o ponto de partida
- **Performance constante** (O(1)) — página 1 e página 1000 custam o mesmo
- Não descarta linhas, vai direto ao ponto
- **Limitação**: não permite ir para uma página arbitrária (só next/previous)

### Comparação Rápida

| Aspecto | OFFSET | KEYSET |
|---|---|---|
| Performance em páginas altas | Lenta | Constante |
| Permite página arbitrária | Sim | Não |
| Uso típico | Dashboards simples | Feeds, listas grandes |
| Complexidade de implementação | Baixa | Média (precisa de índice composto) |

### Deferred Join (otimização híbrida)

Técnica que combina offset com subquery para reduzir o custo:

```sql
SELECT e.* FROM Employees e
INNER JOIN (
    SELECT EmployeeID FROM Employees ORDER BY EmployeeID LIMIT 15 OFFSET 10000
) AS page_ids USING (EmployeeID)
ORDER BY e.EmployeeID;
```

A subquery usa um **covering index** (só `EmployeeID`), examinando menos dados antes do offset. A query externa busca as linhas completas apenas para os 15 IDs encontrados.

---

## Por que Keyset Pagination não permite página arbitrária

O motivo é fundamental: **não existe um "número de página" no keyset** — o cursor é o próprio valor da chave (ex: `EmployeeID > 10000`).

### O problema central

Para ir à **página 50** com keyset, você precisaria saber o **cursor exato** do início da página 50. Mas esse valor só pode ser calculado **percorrendo as páginas anteriores**, ou seja, você teria que executar algo como:

```sql
-- Para chegar à página 50, precisaria saber o último ID da página 49
-- Mas para saber o último ID da página 49, precisa da página 48...
-- E assim por diante até a página 1.
```

É recursivo — o cursor de cada página depende do resultado da anterior.

### Comparação visual

| | OFFSET | KEYSET |
|---|---|---|
| Página 1 | `OFFSET 0` | `WHERE id > 0` |
| Página 50 | `OFFSET 735` (50×15−15) | `WHERE id > ???` |

Com offset, o cálculo é **aritmético**: `página × tamanho`. Com keyset, o valor `???` **só existe no banco após executar as consultas anteriores**.

### Por que existe essa limitação

Keyset assume que os dados são **ordenados e estáveis**. O cursor (`id > 10000`) funciona porque o banco pode usar o índice B-tree para ir direto ao ponto. Mas não existe "índice na posição ordinal" — o banco não sabe que o registro com `id=10000` é o "início da página 500" sem calcular.

### Quando usar cada um

- **Offset**: UI com seletor de página (1, 2, 3... 500) — quando o usuário quer pular direto
- **Keyset**: Feeds infinitos (Twitter, Instagram), streams — quando o usuário só navega sequencialmente (next/previous)
