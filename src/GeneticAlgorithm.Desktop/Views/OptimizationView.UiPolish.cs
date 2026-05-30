using System.Windows.Forms;
using GeneticAlgorithm.Application;
using GeneticAlgorithm.Desktop.Views;
using GeneticAlgorithm.Desktop.Views.Layout;

namespace GeneticAlgorithm.Desktop
{
    public partial class OptimizationView
    {
        private TabPage _tabExhaustiveSearch;
        private TabPage _tabGeneticSearch;
        private TabPage _tabSurrogateSearch;
        private TabPage _tabDynamicProgrammingSearch;

        private void ApplyGlobalUiPolish()
        {
            UiTheme.ApplyForm(this);
            Text = OptimizationPhaseDisplay.ApplicationTitle;
            ApplyControlCopyAndTheme();
            ReorderMainTabs();
        }

        private void ApplyControlCopyAndTheme()
        {
            grpGaInputs.Text = UiCopy.InputsGroup;
            grpSimulationInputs.Text = UiCopy.SimulationInputsGroup;
            grpSimulationUtilizationOutputs.Text = UiCopy.UtilizationOutputsGroup;
            grpSimulationCostOutputs.Text = UiCopy.SimulationOutputsGroup;
            grpDistributionDefaults.Text = UiCopy.DefaultValuesGroup;
            grpDistributionNewValues.Text = UiCopy.NewValuesGroup;

            lblLoadingDistributionHeader.Text = UiCopy.LoadingDistribution;
            lblWeighingDistributionHeader.Text = UiCopy.WeighingDistribution;
            lblTravelingDistributionHeader.Text = UiCopy.TravelingDistribution;

            btnRunGaDemo.Text = UiCopy.RunDemo;
            btnRunGeneticAlgorithm.Text = UiCopy.RunGeneticAlgorithm;
            btnRunSimulation.Text = UiCopy.RunSimulation;
            btnClearSimulationFields.Text = UiCopy.ClearFields;
            btnClearGaFields.Text = UiCopy.ClearFields;
            btnClearDistributionGrids.Text = UiCopy.ClearTables;
            btnAddDistributionsToSimulation.Text = UiCopy.AddToSimulation;

            if (_btnRunSimDemo != null)
                _btnRunSimDemo.Text = UiCopy.RunDemo;
            if (_btnStopGenetic != null)
                _btnStopGenetic.Text = UiCopy.Stop;
            if (_btnStopSim != null)
                _btnStopSim.Text = UiCopy.Stop;

            UiTheme.StyleGroupBox(grpGaInputs);
            UiTheme.StyleGroupBox(grpDistributionNewValues);
            UiTheme.StyleGroupBox(grpDistributionDefaults);
            UiTheme.StyleGroupBox(grpSimulationInputs);
            UiTheme.StyleGroupBox(grpSimulationUtilizationOutputs);
            UiTheme.StyleGroupBox(grpSimulationCostOutputs);

            UiTheme.StylePrimaryButton(btnRunGaDemo);
            UiTheme.StylePrimaryButton(btnRunGeneticAlgorithm);
            UiTheme.StylePrimaryButton(btnRunSimulation);
            UiTheme.StylePrimaryButton(btnAddDistributionsToSimulation);
            UiTheme.StylePrimaryButton(_btnRunSimDemo);

            UiTheme.StyleSecondaryButton(btnClearGaFields);
            UiTheme.StyleSecondaryButton(btnClearSimulationFields);
            UiTheme.StyleSecondaryButton(btnClearDistributionGrids);
            UiTheme.StyleSecondaryButton(_btnStopGenetic);
            UiTheme.StyleSecondaryButton(_btnStopSim);

            UiTheme.StyleDataGrid(gridLoadingDistribution);
            UiTheme.StyleDataGrid(gridWeighingDistribution);
            UiTheme.StyleDataGrid(gridTravelingDistribution);
            ApplyDistributionColumnHeaders();
            UiTheme.StyleSectionLabel(lblLoadingDistributionHeader, title: true);
            UiTheme.StyleSectionLabel(lblWeighingDistributionHeader, title: true);
            UiTheme.StyleSectionLabel(lblTravelingDistributionHeader, title: true);
            UiTheme.StyleChart(chartGaFitness);
            UiTheme.StyleChart(chartSimulationCosts);
        }

        private void ReorderMainTabs()
        {
            if (_tabExhaustiveSearch == null)
                return;

            mainTabControl.SuspendLayout();
            mainTabControl.Controls.Clear();
            mainTabControl.Controls.Add(tabDistribution);
            mainTabControl.Controls.Add(tabSimulation);
            mainTabControl.Controls.Add(tabGeneticAlgorithm);
            mainTabControl.Controls.Add(_tabExhaustiveSearch);
            mainTabControl.Controls.Add(_tabGeneticSearch);
            mainTabControl.Controls.Add(_tabSurrogateSearch);
            mainTabControl.Controls.Add(_tabDynamicProgrammingSearch);
            mainTabControl.ResumeLayout();
        }

        private void ReparentGaClearButton()
        {
            if (btnClearGaFields == null || grpGaInputs == null || tabGeneticAlgorithm == null)
                return;

            if (grpGaInputs.Controls.Contains(btnClearGaFields))
                grpGaInputs.Controls.Remove(btnClearGaFields);

            if (!tabGeneticAlgorithm.Controls.Contains(btnClearGaFields))
                tabGeneticAlgorithm.Controls.Add(btnClearGaFields);
        }

        private TabPage RegisterPhaseTab(string title, PhaseResultPanel panel)
        {
            var page = CreatePhaseTab(title, panel);
            switch (title)
            {
                case OptimizationPhaseDisplay.ExhaustiveSearch:
                    _tabExhaustiveSearch = page;
                    break;
                case OptimizationPhaseDisplay.GeneticSearch:
                    _tabGeneticSearch = page;
                    break;
                case OptimizationPhaseDisplay.SurrogateSearch:
                    _tabSurrogateSearch = page;
                    break;
                case OptimizationPhaseDisplay.DynamicProgrammingSearch:
                    _tabDynamicProgrammingSearch = page;
                    break;
            }

            return page;
        }
    }
}
