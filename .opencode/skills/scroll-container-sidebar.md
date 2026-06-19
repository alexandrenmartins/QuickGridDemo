---
descrição: Container com scroll horizontal em páginas Blazor com sidebar lateral
modo: subagente
ferramentas:
  escrever: verdadeiro
  editar: verdadeiro
---

# Container com Scroll + Sidebar Blazor

Técnicas para adicionar scroll horizontal em conteúdo de páginas Blazor que possuem menu lateral (sidebar), sem afetar o layout do sidebar.

## Estrutura típica do Layout Blazor

```
MainLayout.razor
├── div.page (flex row)
│   ├── div.sidebar          ← NavMenu (fora de @Body)
│   └── main
│       └── article.content
│           └── @Body        ← Renderiza a página (ex: Employees.razor)
```

O `@Body` renderiza **dentro** de `article.content`. Qualquer `<div>` com scroll colocado dentro da página (via `@Body`) está automaticamente escopado ao conteúdo, **não afetando o sidebar**.

## Regra CSS para scroll sem afetar sidebar

```css
.grid-scroll-container {
    overflow: auto;
    width: 0;
    min-width: 100%;
}
```

### Por que `width: 0; min-width: 100%;` e não `max-width: 100%`?

- `max-width: 100%` pode causar overflow no layout flex do `.page`, fazendo o body da página inteira ficar com scroll horizontal (incluindo sidebar)
- `width: 0; min-width: 100%;` força o container a ocupar **exatamente** o espaço disponível dentro de `article.content`, sem estourar o layout

## Previna quebra de linha nas células

Com scroll horizontal, textos longos não devem quebrar linha (senão a row cresce em altura):

```css
.grid-scroll-container table td,
.grid-scroll-container table th {
    white-space: nowrap;
}
```

## Aplicação Razor

```razor
<div class="grid-scroll-container">
    <QuickGrid Items="items.AsQueryable()" Class="table table-striped table-bordered table-hover">
        <PropertyColumn Property="@(x => x.Id)" Title="ID" />
        <PropertyColumn Property="@(x => x.Name)" Title="Nome" />
        <!-- mais colunas -->
    </QuickGrid>

    <!-- Paginador -->
    <div class="d-flex justify-content-between align-items-center mt-2">
        <!-- ... -->
    </div>
</div>
```

## Checklist

1. Colocar o `<div class="grid-scroll-container">` **dentro** da página (após `@page`), não no `MainLayout`
2. Usar `width: 0; min-width: 100%;` (não `max-width: 100%`)
3. Adicionar `white-space: nowrap` em `td` e `th` para evitar quebra de linha
4. O sidebar fica intacto porque está fora de `@Body` no layout

## Referência

- Arquivo: `wwwroot/app.css`
- Arquivo: `Components/Pages/Employees.razor`
- Arquivo: `Components/Layout/MainLayout.razor`
