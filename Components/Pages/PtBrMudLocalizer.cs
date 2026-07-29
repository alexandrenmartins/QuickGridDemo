using Microsoft.Extensions.Localization;
using MudBlazor;

namespace QuickGridDemo.Components.Pages;

internal class PtBrMudLocalizer : MudLocalizer
{
    private readonly Dictionary<string, string> _localization = new()
    {
        // Filter operators
        { "MudDataGrid_AddFilter", "Adicionar filtro" },
        { "MudDataGrid_Apply", "Aplicar" },
        { "MudDataGrid_Cancel", "Cancelar" },
        { "MudDataGrid_Clear", "Limpar" },
        { "MudDataGrid_ClearFilter", "Limpar filtro" },
        { "MudDataGrid_Column", "Coluna" },
        { "MudDataGrid_Columns", "Colunas" },
        { "MudDataGrid_Contains", "Contém" },
        { "MudDataGrid_EndsWith", "Termina com" },
        { "MudDataGrid_Equals", "Igual" },
        { "MudDataGrid_Filter", "Filtro" },
        { "MudDataGrid_FilterValue", "Valor do filtro" },
        { "MudDataGrid_False", "Falso" },
        { "MudDataGrid_Is", "É" },
        { "MudDataGrid_IsAfter", "É depois de" },
        { "MudDataGrid_IsBefore", "É antes de" },
        { "MudDataGrid_IsEmpty", "Está vazio" },
        { "MudDataGrid_IsNot", "Não é" },
        { "MudDataGrid_IsNotEmpty", "Não está vazio" },
        { "MudDataGrid_IsOnOrAfter", "É igual ou depois de" },
        { "MudDataGrid_IsOnOrBefore", "É igual ou antes de" },
        { "MudDataGrid_NotContains", "Não contém" },
        { "MudDataGrid_NotEquals", "Diferente" },
        { "MudDataGrid_OpenFilters", "Abrir filtros" },
        { "MudDataGrid_Operator", "Operador" },
        { "MudDataGrid_RemoveFilter", "Remover filtro" },
        { "MudDataGrid_StartsWith", "Começa com" },
        { "MudDataGrid_True", "Verdadeiro" },
        { "MudDataGrid_Value", "Valor" },
        { "MudDataGrid_Unsort", "Sem ordenação" },
        { "MudDataGrid_Sort", "Ordenar" },
        { "MudDataGrid_RefreshData", "Atualizar dados" },
        { "MudDataGrid_Save", "Salvar" },
        { "MudDataGrid_Loading", "Carregando..." },

        // Grouping
        { "MudDataGrid_CollapseAllGroups", "Recolher todos os grupos" },
        { "MudDataGrid_ExpandAllGroups", "Expandir todos os grupos" },
        { "MudDataGrid_Group", "Agrupar" },
        { "MudDataGrid_Ungroup", "Desagrupar" },

        // Column visibility
        { "MudDataGrid_Hide", "Ocultar" },
        { "MudDataGrid_HideAll", "Ocultar todos" },
        { "MudDataGrid_ShowAll", "Mostrar todos" },

        // Reorder
        { "MudDataGrid_MoveUp", "Mover para cima" },
        { "MudDataGrid_MoveDown", "Mover para baixo" },

        // Signs
        { "MudDataGrid_EqualSign", "=" },
        { "MudDataGrid_NotEqualSign", "!=" },
        { "MudDataGrid_GreaterThanSign", ">" },
        { "MudDataGrid_GreaterThanOrEqualSign", ">=" },
        { "MudDataGrid_LessThanSign", "<" },
        { "MudDataGrid_LessThanOrEqualSign", "<=" },

        // Pager
        { "MudDataGridPager_AllItems", "Todos" },
        { "MudDataGridPager_FirstPage", "Primeira página" },
        { "MudDataGridPager_LastPage", "Última página" },
        { "MudDataGridPager_NextPage", "Próxima página" },
        { "MudDataGridPager_PreviousPage", "Página anterior" },
        { "MudDataGridPager_ItemsPerPage", "Itens por página" },
        { "MudDataGridPager_RowsPerPage", "Linhas por página:" },
        { "MudDataGridPager_InfoFormat", "{0}-{1} de {2}" },
    };

    public override LocalizedString this[string key]
    {
        get
        {
            if (_localization.TryGetValue(key, out var res))
                return new LocalizedString(key, res);
            return new LocalizedString(key, key, true);
        }
    }
}
