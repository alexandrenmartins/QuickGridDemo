# Tabelas e Relacionamentos

## Tabelas

### Tabela: `Empresa`

- `id_empresa` (PK): Identificador único da empresa.
- `nome_empresa`: Nome da empresa.
- `cnpj`: CNPJ da empresa.
- `endereco`: Endereço da empresa.
- `telefone`: Telefone de contato da empresa.
- `email`: E-mail de contato da empresa.
- `data_criacao`: Data de criação do registro.
- `data_atualizacao`: Data da última atualização do registro.
- `status`: Status da empresa (ativo/inativo).

### Tabela: `Cliente`

- `id_cliente` (PK): Identificador único do cliente.
- `nome_cliente`: Nome do cliente.
- `cpf`: CPF do cliente.
- `email`: E-mail do cliente.
- `telefone`: Telefone de contato do cliente.

### Tabela: `EmpresaCliente`

- `id_empresa` (PK): Identificador único da empresa do relacionamento entre empresa e cliente.
- `id_cliente` (FK): Identificador do cliente relacionado.

### Tabela: `Cargo`

- `id_cargo` (PK): Identificador único do cargo.
- `nome_cargo`: Nome do cargo.
- `descricao`: Descrição do cargo.
- `salario_base`: Salário associado ao cargo.

### Tabela: `Funcionario`

- `id_funcionario` (PK): Identificador único do funcionário.
- `id_cargo` (FK): Identificador do cargo associado ao funcionário.
- `id_empresa` (FK): Identificador da empresa à qual o funcionário pertence.
- `nome_funcionario`: Nome do funcionário.
- `data_admissao`: Data de admissão do funcionário.
- `salario`: Salário do funcionário.

### Tabela: `Endereco`

- `id_endereco` (PK): Identificador único do endereço.
- `id_cliente` (FK): Identificador do cliente associado ao endereço.
- `logradouro`: Logradouro do endereço.
- `numero`: Número do endereço.
- `complemento`: Complemento do endereço.
- `bairro`: Bairro do endereço.
- `cidade`: Cidade do endereço.
- `estado`: Estado do endereço.
- `cep`: CEP do endereço.
- `pais`: País do endereço.
- `tipo_endereco`: Tipo de endereço (residencial, comercial, etc.).
- `data_criacao`: Data de criação do registro.

### Tabela: `Pedido`

- `id_pedido` (PK): Identificador único do pedido.
- `id_cliente` (FK): Identificador do cliente que fez o pedido.
- `id_funcionario` (FK): Identificador do funcionário que processou o pedido.
- `data_pedido`: Data em que o pedido foi realizado.
- `status_pedido`: Status atual do pedido (pendente, em processamento, concluído, cancelado).

### Tabela: `ItemPedido`

- `id_item` (PK): Identificador único do item do pedido.
- `id_pedido` (FK): Identificador do pedido ao qual o item pertence.
- `id_produto` (FK): Identificador do produto associado ao item do pedido.
- `quantidade`: Quantidade do produto no item do pedido.
- `preco_unitario`: Preço unitário do produto no item do pedido.

### Tabela: `Produto`

- `id_produto` (PK): Identificador único do produto.
- `id_categoria` (FK): Identificador da categoria associada ao produto.
- `nome_produto`: Nome do produto.
- `preco`: Preço do produto.
- `estoque`: Quantidade em estoque do produto.

### Tabela: `Categoria`

- `id_categoria` (PK): Identificador único da categoria.
- `nome_categoria`: Nome da categoria.

### Tabela: `Pagamento`

- `id_pagamento` (PK): Identificador único do pagamento.
- `id_pedido` (FK): Identificador do pedido associado ao pagamento.
- `data_pagamento`: Data em que o pagamento foi realizado.
- `valor`: Valor do pagamento.
- `metodo_pagamento`: Método de pagamento utilizado (cartão, boleto, transferência, etc.).

## Relacionamentos

| Relacionamento | Tabelas | Tipo |
| --- | --- | --- |
| Empresa ↔ Funcionario | 1:N | Uma empresa tem vários funcionários |
| Empresa ↔ Cliente (via EmpresaCliente) | N:M | Uma empresa atende vários clientes |
| Cargo ↔ Funcionario | 1:N | Um cargo pode ter vários funcionários |
| Cliente ↔ Endereco | 1:N | Um cliente tem vários endereços |
| Cliente ↔ Pedido | 1:N | Um cliente faz vários pedidos |
| Funcionario ↔ Pedido | 1:N | Um funcionário atende vários pedidos |
| Pedido ↔ Pagamento | 1:1 | Cada pedido tem um pagamento único |
| Pedido ↔ ItemPedido | 1:N | Um pedido contém vários itens |
| Produto ↔ ItemPedido | 1:N | Um produto aparece em vários itens de pedido |
| Categoria ↔ Produto | 1:N | Uma categoria pode ter vários produtos |