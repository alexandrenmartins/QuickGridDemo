using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace QuickGridDemo.Data.Migrations.PostgreSqlMigrations
{
    /// <inheritdoc />
    public partial class AddEntidadeRelacionamentoTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cargos",
                columns: table => new
                {
                    id_cargo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nome_cargo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    descricao = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    salario_base = table.Column<decimal>(type: "numeric(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cargos", x => x.id_cargo);
                });

            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    id_categoria = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nome_categoria = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.id_categoria);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    id_cliente = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nome_cliente = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    cpf = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    telefone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.id_cliente);
                });

            migrationBuilder.CreateTable(
                name: "Empresas",
                columns: table => new
                {
                    id_empresa = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nome_empresa = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    cnpj = table.Column<string>(type: "character varying(18)", maxLength: 18, nullable: false),
                    endereco = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    telefone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    data_criacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    data_atualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    status = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresas", x => x.id_empresa);
                });

            migrationBuilder.CreateTable(
                name: "Produtos",
                columns: table => new
                {
                    id_produto = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_categoria = table.Column<int>(type: "integer", nullable: false),
                    nome_produto = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    preco = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    estoque = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produtos", x => x.id_produto);
                    table.ForeignKey(
                        name: "FK_Produtos_Categorias_id_categoria",
                        column: x => x.id_categoria,
                        principalTable: "Categorias",
                        principalColumn: "id_categoria",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Enderecos",
                columns: table => new
                {
                    id_endereco = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_cliente = table.Column<int>(type: "integer", nullable: false),
                    logradouro = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    numero = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    complemento = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    bairro = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    cidade = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    estado = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    cep = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    pais = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    tipo_endereco = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    data_criacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enderecos", x => x.id_endereco);
                    table.ForeignKey(
                        name: "FK_Enderecos_Clientes_id_cliente",
                        column: x => x.id_cliente,
                        principalTable: "Clientes",
                        principalColumn: "id_cliente",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmpresaClientes",
                columns: table => new
                {
                    id_empresa = table.Column<int>(type: "integer", nullable: false),
                    id_cliente = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmpresaClientes", x => new { x.id_empresa, x.id_cliente });
                    table.ForeignKey(
                        name: "FK_EmpresaClientes_Clientes_id_cliente",
                        column: x => x.id_cliente,
                        principalTable: "Clientes",
                        principalColumn: "id_cliente",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmpresaClientes_Empresas_id_empresa",
                        column: x => x.id_empresa,
                        principalTable: "Empresas",
                        principalColumn: "id_empresa",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Funcionarios",
                columns: table => new
                {
                    id_funcionario = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_cargo = table.Column<int>(type: "integer", nullable: false),
                    id_empresa = table.Column<int>(type: "integer", nullable: false),
                    nome_funcionario = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    data_admissao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    salario = table.Column<decimal>(type: "numeric(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funcionarios", x => x.id_funcionario);
                    table.ForeignKey(
                        name: "FK_Funcionarios_Cargos_id_cargo",
                        column: x => x.id_cargo,
                        principalTable: "Cargos",
                        principalColumn: "id_cargo",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Funcionarios_Empresas_id_empresa",
                        column: x => x.id_empresa,
                        principalTable: "Empresas",
                        principalColumn: "id_empresa",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pedidos",
                columns: table => new
                {
                    id_pedido = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_cliente = table.Column<int>(type: "integer", nullable: false),
                    id_funcionario = table.Column<int>(type: "integer", nullable: false),
                    data_pedido = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    status_pedido = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos", x => x.id_pedido);
                    table.ForeignKey(
                        name: "FK_Pedidos_Clientes_id_cliente",
                        column: x => x.id_cliente,
                        principalTable: "Clientes",
                        principalColumn: "id_cliente",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pedidos_Funcionarios_id_funcionario",
                        column: x => x.id_funcionario,
                        principalTable: "Funcionarios",
                        principalColumn: "id_funcionario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItensPedido",
                columns: table => new
                {
                    id_item = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_pedido = table.Column<int>(type: "integer", nullable: false),
                    id_produto = table.Column<int>(type: "integer", nullable: false),
                    quantidade = table.Column<int>(type: "integer", nullable: false),
                    preco_unitario = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensPedido", x => x.id_item);
                    table.ForeignKey(
                        name: "FK_ItensPedido_Pedidos_id_pedido",
                        column: x => x.id_pedido,
                        principalTable: "Pedidos",
                        principalColumn: "id_pedido",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItensPedido_Produtos_id_produto",
                        column: x => x.id_produto,
                        principalTable: "Produtos",
                        principalColumn: "id_produto",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pagamentos",
                columns: table => new
                {
                    id_pagamento = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_pedido = table.Column<int>(type: "integer", nullable: false),
                    data_pagamento = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    valor = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    metodo_pagamento = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagamentos", x => x.id_pagamento);
                    table.ForeignKey(
                        name: "FK_Pagamentos_Pedidos_id_pedido",
                        column: x => x.id_pedido,
                        principalTable: "Pedidos",
                        principalColumn: "id_pedido",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmpresaClientes_id_cliente",
                table: "EmpresaClientes",
                column: "id_cliente");

            migrationBuilder.CreateIndex(
                name: "IX_Enderecos_id_cliente",
                table: "Enderecos",
                column: "id_cliente");

            migrationBuilder.CreateIndex(
                name: "IX_Funcionarios_id_cargo",
                table: "Funcionarios",
                column: "id_cargo");

            migrationBuilder.CreateIndex(
                name: "IX_Funcionarios_id_empresa",
                table: "Funcionarios",
                column: "id_empresa");

            migrationBuilder.CreateIndex(
                name: "IX_ItensPedido_id_pedido",
                table: "ItensPedido",
                column: "id_pedido");

            migrationBuilder.CreateIndex(
                name: "IX_ItensPedido_id_produto",
                table: "ItensPedido",
                column: "id_produto");

            migrationBuilder.CreateIndex(
                name: "IX_Pagamentos_id_pedido",
                table: "Pagamentos",
                column: "id_pedido",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_id_cliente",
                table: "Pedidos",
                column: "id_cliente");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_id_funcionario",
                table: "Pedidos",
                column: "id_funcionario");

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_id_categoria",
                table: "Produtos",
                column: "id_categoria");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmpresaClientes");

            migrationBuilder.DropTable(
                name: "Enderecos");

            migrationBuilder.DropTable(
                name: "ItensPedido");

            migrationBuilder.DropTable(
                name: "Pagamentos");

            migrationBuilder.DropTable(
                name: "Produtos");

            migrationBuilder.DropTable(
                name: "Pedidos");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Funcionarios");

            migrationBuilder.DropTable(
                name: "Cargos");

            migrationBuilder.DropTable(
                name: "Empresas");
        }
    }
}
