namespace GeneticAlgorithm.Desktop
{
    partial class OptimizationView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.Title title2 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.Title title3 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptimizationView));
            this.tabGeneticAlgorithm = new System.Windows.Forms.TabPage();
            this.grpLegacyGaOutputs = new System.Windows.Forms.GroupBox();
            this.lblLegacyGaTotalCost = new System.Windows.Forms.Label();
            this.lblLegacyGaTotalDays = new System.Windows.Forms.Label();
            this.lblLegacyGaDelayCost = new System.Windows.Forms.Label();
            this.lblLegacyGaDaysDelayed = new System.Windows.Forms.Label();
            this.lblLegacyGaDaysDelayedCaption = new System.Windows.Forms.Label();
            this.lblLegacyGaTotalCostCaption = new System.Windows.Forms.Label();
            this.lblLegacyGaTotalDaysCaption = new System.Windows.Forms.Label();
            this.lblLegacyGaDelayCostCaption = new System.Windows.Forms.Label();
            this.lblLegacyGaUtilScalers = new System.Windows.Forms.Label();
            this.lblLegacyGaUtilScalersCaption = new System.Windows.Forms.Label();
            this.lblLegacyGaUtilLoaders = new System.Windows.Forms.Label();
            this.lblLegacyGaUtilTrucks = new System.Windows.Forms.Label();
            this.lblLegacyGaScalerCount = new System.Windows.Forms.Label();
            this.lblLegacyGaUtilLoadersCaption = new System.Windows.Forms.Label();
            this.lblLegacyGaUtilTrucksCaption = new System.Windows.Forms.Label();
            this.lblLegacyGaScalerCountCaption = new System.Windows.Forms.Label();
            this.lblLegacyGaLoaderCountCaption = new System.Windows.Forms.Label();
            this.lblLegacyGaTruckCountCaption = new System.Windows.Forms.Label();
            this.lblLegacyGaLoaderCount = new System.Windows.Forms.Label();
            this.lblLegacyGaTruckCount = new System.Windows.Forms.Label();
            this.btnRunGeneticAlgorithm = new System.Windows.Forms.Button();
            this.lblGaProgressCaption = new System.Windows.Forms.Label();
            this.progressGaRun = new System.Windows.Forms.ProgressBar();
            this.grpGaInputs = new System.Windows.Forms.GroupBox();
            this.btnClearGaFields = new System.Windows.Forms.Button();
            this.lblGaMaterialVolume = new System.Windows.Forms.Label();
            this.lblGaLoaderCostPerDay = new System.Windows.Forms.Label();
            this.lblGaScalerCostPerDay = new System.Windows.Forms.Label();
            this.txtGaGenerations = new System.Windows.Forms.TextBox();
            this.lblGaTruckCostPerDay = new System.Windows.Forms.Label();
            this.lblGaPopulation = new System.Windows.Forms.Label();
            this.lblGaLoadPerTruck = new System.Windows.Forms.Label();
            this.txtGaMutationRate = new System.Windows.Forms.TextBox();
            this.lblGaProjectDuration = new System.Windows.Forms.Label();
            this.lblGaGenerations = new System.Windows.Forms.Label();
            this.lblGaDelayCostPerDay = new System.Windows.Forms.Label();
            this.txtGaPopulation = new System.Windows.Forms.TextBox();
            this.lblGaMutationRate = new System.Windows.Forms.Label();
            this.txtGaMaterialVolume = new System.Windows.Forms.TextBox();
            this.txtGaLoadPerTruck = new System.Windows.Forms.TextBox();
            this.txtGaMaxScalers = new System.Windows.Forms.TextBox();
            this.txtGaTruckCostPerDay = new System.Windows.Forms.TextBox();
            this.lblGaMaxScalers = new System.Windows.Forms.Label();
            this.txtGaLoaderCostPerDay = new System.Windows.Forms.TextBox();
            this.txtGaMaxLoaders = new System.Windows.Forms.TextBox();
            this.txtGaScalerCostPerDay = new System.Windows.Forms.TextBox();
            this.lblGaMaxLoaders = new System.Windows.Forms.Label();
            this.txtGaProjectDuration = new System.Windows.Forms.TextBox();
            this.txtGaMaxTrucks = new System.Windows.Forms.TextBox();
            this.txtGaDelayCostPerDay = new System.Windows.Forms.TextBox();
            this.lblGaMaxTrucks = new System.Windows.Forms.Label();
            this.btnRunGaDemo = new System.Windows.Forms.Button();
            this.chartGaFitness = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.picGaHeader = new System.Windows.Forms.PictureBox();
            this.tabSimulation = new System.Windows.Forms.TabPage();
            this.chartSimulationCosts = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.btnClearSimulationFields = new System.Windows.Forms.Button();
            this.grpSimulationCostOutputs = new System.Windows.Forms.GroupBox();
            this.lblSimDaysDelayedValue = new System.Windows.Forms.Label();
            this.lblSimDaysDelayedCaption = new System.Windows.Forms.Label();
            this.lblSimProjectDurationCaption = new System.Windows.Forms.Label();
            this.lblSimLoaderCostCaption = new System.Windows.Forms.Label();
            this.lblSimScalerCostCaption = new System.Windows.Forms.Label();
            this.lblSimTotalCostValue = new System.Windows.Forms.Label();
            this.lblSimDelayCostCaption = new System.Windows.Forms.Label();
            this.lblSimTotalCostCaption = new System.Windows.Forms.Label();
            this.lblSimTruckCostCaption = new System.Windows.Forms.Label();
            this.lblSimLoaderCostValue = new System.Windows.Forms.Label();
            this.lblSimTruckCostValue = new System.Windows.Forms.Label();
            this.lblSimDelayCostValue = new System.Windows.Forms.Label();
            this.lblSimScalerCostValue = new System.Windows.Forms.Label();
            this.lblSimProjectDurationValue = new System.Windows.Forms.Label();
            this.grpSimulationUtilizationOutputs = new System.Windows.Forms.GroupBox();
            this.lblSimUtilTrucksCaption = new System.Windows.Forms.Label();
            this.lblSimUtilScalersCaption = new System.Windows.Forms.Label();
            this.lblSimUtilLoadersCaption = new System.Windows.Forms.Label();
            this.lblSimUtilTrucksValue = new System.Windows.Forms.Label();
            this.lblSimUtilLoadersValue = new System.Windows.Forms.Label();
            this.lblSimUtilScalersValue = new System.Windows.Forms.Label();
            this.grpSimulationInputs = new System.Windows.Forms.GroupBox();
            this.txtSimScalerCount = new System.Windows.Forms.TextBox();
            this.lblSimScalerCount = new System.Windows.Forms.Label();
            this.txtSimLoaderCount = new System.Windows.Forms.TextBox();
            this.lblSimLoaderCount = new System.Windows.Forms.Label();
            this.txtSimTruckCount = new System.Windows.Forms.TextBox();
            this.lblSimTruckCount = new System.Windows.Forms.Label();
            this.txtSimLoaderCostPerDay = new System.Windows.Forms.TextBox();
            this.txtSimDelayCostPerDay = new System.Windows.Forms.TextBox();
            this.txtSimProjectDuration = new System.Windows.Forms.TextBox();
            this.txtSimScalerCostPerDay = new System.Windows.Forms.TextBox();
            this.txtSimTruckCostPerDay = new System.Windows.Forms.TextBox();
            this.txtSimLoadPerTruck = new System.Windows.Forms.TextBox();
            this.txtSimMaterialVolume = new System.Windows.Forms.TextBox();
            this.lblSimMaterialVolume = new System.Windows.Forms.Label();
            this.lblSimDelayCostPerDay = new System.Windows.Forms.Label();
            this.lblSimProjectDuration = new System.Windows.Forms.Label();
            this.lblSimLoadPerTruck = new System.Windows.Forms.Label();
            this.lblSimTruckCostPerDay = new System.Windows.Forms.Label();
            this.lblSimScalerCostPerDay = new System.Windows.Forms.Label();
            this.lblSimLoaderCostPerDay = new System.Windows.Forms.Label();
            this.btnRunSimulation = new System.Windows.Forms.Button();
            this.picSimulationHeader = new System.Windows.Forms.PictureBox();
            this.tabDistribution = new System.Windows.Forms.TabPage();
            this.grpDistributionDefaults = new System.Windows.Forms.GroupBox();
            this.picDefaultLoadingDistribution = new System.Windows.Forms.PictureBox();
            this.picDefaultTravelDistribution = new System.Windows.Forms.PictureBox();
            this.picDefaultWeighingDistribution = new System.Windows.Forms.PictureBox();
            this.grpDistributionNewValues = new System.Windows.Forms.GroupBox();
            this.lblTravelingDistributionHeader = new System.Windows.Forms.Label();
            this.lblWeighingDistributionHeader = new System.Windows.Forms.Label();
            this.lblLoadingDistributionHeader = new System.Windows.Forms.Label();
            this.btnClearDistributionGrids = new System.Windows.Forms.Button();
            this.btnAddDistributionsToSimulation = new System.Windows.Forms.Button();
            this.gridTravelingDistribution = new System.Windows.Forms.DataGridView();
            this.colTravelingTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTravelingProbability = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridWeighingDistribution = new System.Windows.Forms.DataGridView();
            this.colWeighingTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWeighingProbability = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridLoadingDistribution = new System.Windows.Forms.DataGridView();
            this.colLoadingTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLoadingProbability = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mainTabControl = new System.Windows.Forms.TabControl();
            this.tabGeneticAlgorithm.SuspendLayout();
            this.grpLegacyGaOutputs.SuspendLayout();
            this.grpGaInputs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartGaFitness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGaHeader)).BeginInit();
            this.tabSimulation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartSimulationCosts)).BeginInit();
            this.grpSimulationCostOutputs.SuspendLayout();
            this.grpSimulationUtilizationOutputs.SuspendLayout();
            this.grpSimulationInputs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSimulationHeader)).BeginInit();
            this.tabDistribution.SuspendLayout();
            this.grpDistributionDefaults.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picDefaultLoadingDistribution)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDefaultTravelDistribution)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDefaultWeighingDistribution)).BeginInit();
            this.grpDistributionNewValues.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridTravelingDistribution)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridWeighingDistribution)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLoadingDistribution)).BeginInit();
            this.mainTabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabGeneticAlgorithm
            // 
            this.tabGeneticAlgorithm.Controls.Add(this.grpLegacyGaOutputs);
            this.tabGeneticAlgorithm.Controls.Add(this.btnRunGeneticAlgorithm);
            this.tabGeneticAlgorithm.Controls.Add(this.lblGaProgressCaption);
            this.tabGeneticAlgorithm.Controls.Add(this.progressGaRun);
            this.tabGeneticAlgorithm.Controls.Add(this.grpGaInputs);
            this.tabGeneticAlgorithm.Controls.Add(this.btnRunGaDemo);
            this.tabGeneticAlgorithm.Controls.Add(this.chartGaFitness);
            this.tabGeneticAlgorithm.Controls.Add(this.picGaHeader);
            this.tabGeneticAlgorithm.Location = new System.Drawing.Point(4, 22);
            this.tabGeneticAlgorithm.Name = "tabGeneticAlgorithm";
            this.tabGeneticAlgorithm.Padding = new System.Windows.Forms.Padding(3);
            this.tabGeneticAlgorithm.Size = new System.Drawing.Size(1312, 752);
            this.tabGeneticAlgorithm.TabIndex = 0;
            this.tabGeneticAlgorithm.Text = "Genetic Algorithm";
            this.tabGeneticAlgorithm.UseVisualStyleBackColor = true;
            // 
            // grpLegacyGaOutputs
            // 
            this.grpLegacyGaOutputs.Controls.Add(this.lblLegacyGaTotalCost);
            this.grpLegacyGaOutputs.Controls.Add(this.lblLegacyGaTotalDays);
            this.grpLegacyGaOutputs.Controls.Add(this.lblLegacyGaDelayCost);
            this.grpLegacyGaOutputs.Controls.Add(this.lblLegacyGaDaysDelayed);
            this.grpLegacyGaOutputs.Controls.Add(this.lblLegacyGaDaysDelayedCaption);
            this.grpLegacyGaOutputs.Controls.Add(this.lblLegacyGaTotalCostCaption);
            this.grpLegacyGaOutputs.Controls.Add(this.lblLegacyGaTotalDaysCaption);
            this.grpLegacyGaOutputs.Controls.Add(this.lblLegacyGaDelayCostCaption);
            this.grpLegacyGaOutputs.Controls.Add(this.lblLegacyGaUtilScalers);
            this.grpLegacyGaOutputs.Controls.Add(this.lblLegacyGaUtilScalersCaption);
            this.grpLegacyGaOutputs.Controls.Add(this.lblLegacyGaUtilLoaders);
            this.grpLegacyGaOutputs.Controls.Add(this.lblLegacyGaUtilTrucks);
            this.grpLegacyGaOutputs.Controls.Add(this.lblLegacyGaScalerCount);
            this.grpLegacyGaOutputs.Controls.Add(this.lblLegacyGaUtilLoadersCaption);
            this.grpLegacyGaOutputs.Controls.Add(this.lblLegacyGaUtilTrucksCaption);
            this.grpLegacyGaOutputs.Controls.Add(this.lblLegacyGaScalerCountCaption);
            this.grpLegacyGaOutputs.Controls.Add(this.lblLegacyGaLoaderCountCaption);
            this.grpLegacyGaOutputs.Controls.Add(this.lblLegacyGaTruckCountCaption);
            this.grpLegacyGaOutputs.Controls.Add(this.lblLegacyGaLoaderCount);
            this.grpLegacyGaOutputs.Controls.Add(this.lblLegacyGaTruckCount);
            this.grpLegacyGaOutputs.Location = new System.Drawing.Point(425, 318);
            this.grpLegacyGaOutputs.Margin = new System.Windows.Forms.Padding(2);
            this.grpLegacyGaOutputs.Name = "grpLegacyGaOutputs";
            this.grpLegacyGaOutputs.Padding = new System.Windows.Forms.Padding(2);
            this.grpLegacyGaOutputs.Size = new System.Drawing.Size(428, 195);
            this.grpLegacyGaOutputs.TabIndex = 72;
            this.grpLegacyGaOutputs.TabStop = false;
            this.grpLegacyGaOutputs.Text = "Algorithm OutPuts ";
            // 
            // lblLegacyGaTotalCost
            // 
            this.lblLegacyGaTotalCost.AutoSize = true;
            this.lblLegacyGaTotalCost.Location = new System.Drawing.Point(343, 162);
            this.lblLegacyGaTotalCost.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLegacyGaTotalCost.Name = "lblLegacyGaTotalCost";
            this.lblLegacyGaTotalCost.Size = new System.Drawing.Size(61, 13);
            this.lblLegacyGaTotalCost.TabIndex = 91;
            this.lblLegacyGaTotalCost.Text = "..................";
            // 
            // lblLegacyGaTotalDays
            // 
            this.lblLegacyGaTotalDays.AutoSize = true;
            this.lblLegacyGaTotalDays.Location = new System.Drawing.Point(343, 128);
            this.lblLegacyGaTotalDays.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLegacyGaTotalDays.Name = "lblLegacyGaTotalDays";
            this.lblLegacyGaTotalDays.Size = new System.Drawing.Size(61, 13);
            this.lblLegacyGaTotalDays.TabIndex = 90;
            this.lblLegacyGaTotalDays.Text = "..................";
            // 
            // lblLegacyGaDelayCost
            // 
            this.lblLegacyGaDelayCost.AutoSize = true;
            this.lblLegacyGaDelayCost.Location = new System.Drawing.Point(343, 95);
            this.lblLegacyGaDelayCost.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLegacyGaDelayCost.Name = "lblLegacyGaDelayCost";
            this.lblLegacyGaDelayCost.Size = new System.Drawing.Size(61, 13);
            this.lblLegacyGaDelayCost.TabIndex = 89;
            this.lblLegacyGaDelayCost.Text = "..................";
            // 
            // lblLegacyGaDaysDelayed
            // 
            this.lblLegacyGaDaysDelayed.AutoSize = true;
            this.lblLegacyGaDaysDelayed.Location = new System.Drawing.Point(343, 62);
            this.lblLegacyGaDaysDelayed.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLegacyGaDaysDelayed.Name = "lblLegacyGaDaysDelayed";
            this.lblLegacyGaDaysDelayed.Size = new System.Drawing.Size(61, 13);
            this.lblLegacyGaDaysDelayed.TabIndex = 88;
            this.lblLegacyGaDaysDelayed.Text = "..................";
            // 
            // lblLegacyGaDaysDelayedCaption
            // 
            this.lblLegacyGaDaysDelayedCaption.AutoSize = true;
            this.lblLegacyGaDaysDelayedCaption.Location = new System.Drawing.Point(230, 62);
            this.lblLegacyGaDaysDelayedCaption.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLegacyGaDaysDelayedCaption.Name = "lblLegacyGaDaysDelayedCaption";
            this.lblLegacyGaDaysDelayedCaption.Size = new System.Drawing.Size(76, 13);
            this.lblLegacyGaDaysDelayedCaption.TabIndex = 87;
            this.lblLegacyGaDaysDelayedCaption.Text = "Days Delayed:";
            // 
            // lblLegacyGaTotalCostCaption
            // 
            this.lblLegacyGaTotalCostCaption.AutoSize = true;
            this.lblLegacyGaTotalCostCaption.Location = new System.Drawing.Point(230, 162);
            this.lblLegacyGaTotalCostCaption.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLegacyGaTotalCostCaption.Name = "lblLegacyGaTotalCostCaption";
            this.lblLegacyGaTotalCostCaption.Size = new System.Drawing.Size(58, 13);
            this.lblLegacyGaTotalCostCaption.TabIndex = 86;
            this.lblLegacyGaTotalCostCaption.Text = "Total Cost:";
            // 
            // lblLegacyGaTotalDaysCaption
            // 
            this.lblLegacyGaTotalDaysCaption.AutoSize = true;
            this.lblLegacyGaTotalDaysCaption.Location = new System.Drawing.Point(230, 128);
            this.lblLegacyGaTotalDaysCaption.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLegacyGaTotalDaysCaption.Name = "lblLegacyGaTotalDaysCaption";
            this.lblLegacyGaTotalDaysCaption.Size = new System.Drawing.Size(61, 13);
            this.lblLegacyGaTotalDaysCaption.TabIndex = 85;
            this.lblLegacyGaTotalDaysCaption.Text = "Total Days:";
            // 
            // lblLegacyGaDelayCostCaption
            // 
            this.lblLegacyGaDelayCostCaption.AutoSize = true;
            this.lblLegacyGaDelayCostCaption.Location = new System.Drawing.Point(230, 95);
            this.lblLegacyGaDelayCostCaption.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLegacyGaDelayCostCaption.Name = "lblLegacyGaDelayCostCaption";
            this.lblLegacyGaDelayCostCaption.Size = new System.Drawing.Size(73, 13);
            this.lblLegacyGaDelayCostCaption.TabIndex = 84;
            this.lblLegacyGaDelayCostCaption.Text = "Cost of Delay:";
            // 
            // lblLegacyGaUtilScalers
            // 
            this.lblLegacyGaUtilScalers.AutoSize = true;
            this.lblLegacyGaUtilScalers.Location = new System.Drawing.Point(343, 28);
            this.lblLegacyGaUtilScalers.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLegacyGaUtilScalers.Name = "lblLegacyGaUtilScalers";
            this.lblLegacyGaUtilScalers.Size = new System.Drawing.Size(61, 13);
            this.lblLegacyGaUtilScalers.TabIndex = 83;
            this.lblLegacyGaUtilScalers.Text = "..................";
            // 
            // lblLegacyGaUtilScalersCaption
            // 
            this.lblLegacyGaUtilScalersCaption.AutoSize = true;
            this.lblLegacyGaUtilScalersCaption.Location = new System.Drawing.Point(230, 28);
            this.lblLegacyGaUtilScalersCaption.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLegacyGaUtilScalersCaption.Name = "lblLegacyGaUtilScalersCaption";
            this.lblLegacyGaUtilScalersCaption.Size = new System.Drawing.Size(105, 13);
            this.lblLegacyGaUtilScalersCaption.TabIndex = 82;
            this.lblLegacyGaUtilScalersCaption.Text = "Utilization of Scalers:";
            // 
            // lblLegacyGaUtilLoaders
            // 
            this.lblLegacyGaUtilLoaders.AutoSize = true;
            this.lblLegacyGaUtilLoaders.Location = new System.Drawing.Point(128, 162);
            this.lblLegacyGaUtilLoaders.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLegacyGaUtilLoaders.Name = "lblLegacyGaUtilLoaders";
            this.lblLegacyGaUtilLoaders.Size = new System.Drawing.Size(61, 13);
            this.lblLegacyGaUtilLoaders.TabIndex = 81;
            this.lblLegacyGaUtilLoaders.Text = "..................";
            // 
            // lblLegacyGaUtilTrucks
            // 
            this.lblLegacyGaUtilTrucks.AutoSize = true;
            this.lblLegacyGaUtilTrucks.Location = new System.Drawing.Point(126, 128);
            this.lblLegacyGaUtilTrucks.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLegacyGaUtilTrucks.Name = "lblLegacyGaUtilTrucks";
            this.lblLegacyGaUtilTrucks.Size = new System.Drawing.Size(61, 13);
            this.lblLegacyGaUtilTrucks.TabIndex = 80;
            this.lblLegacyGaUtilTrucks.Text = "..................";
            // 
            // lblLegacyGaScalerCount
            // 
            this.lblLegacyGaScalerCount.AutoSize = true;
            this.lblLegacyGaScalerCount.Location = new System.Drawing.Point(126, 95);
            this.lblLegacyGaScalerCount.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLegacyGaScalerCount.Name = "lblLegacyGaScalerCount";
            this.lblLegacyGaScalerCount.Size = new System.Drawing.Size(61, 13);
            this.lblLegacyGaScalerCount.TabIndex = 79;
            this.lblLegacyGaScalerCount.Text = "..................";
            // 
            // lblLegacyGaUtilLoadersCaption
            // 
            this.lblLegacyGaUtilLoadersCaption.AutoSize = true;
            this.lblLegacyGaUtilLoadersCaption.Location = new System.Drawing.Point(14, 162);
            this.lblLegacyGaUtilLoadersCaption.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLegacyGaUtilLoadersCaption.Name = "lblLegacyGaUtilLoadersCaption";
            this.lblLegacyGaUtilLoadersCaption.Size = new System.Drawing.Size(108, 13);
            this.lblLegacyGaUtilLoadersCaption.TabIndex = 78;
            this.lblLegacyGaUtilLoadersCaption.Text = "Utilization of Loaders:";
            // 
            // lblLegacyGaUtilTrucksCaption
            // 
            this.lblLegacyGaUtilTrucksCaption.AutoSize = true;
            this.lblLegacyGaUtilTrucksCaption.Location = new System.Drawing.Point(14, 128);
            this.lblLegacyGaUtilTrucksCaption.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLegacyGaUtilTrucksCaption.Name = "lblLegacyGaUtilTrucksCaption";
            this.lblLegacyGaUtilTrucksCaption.Size = new System.Drawing.Size(103, 13);
            this.lblLegacyGaUtilTrucksCaption.TabIndex = 77;
            this.lblLegacyGaUtilTrucksCaption.Text = "Utilization of Trucks:";
            // 
            // lblLegacyGaScalerCountCaption
            // 
            this.lblLegacyGaScalerCountCaption.AutoSize = true;
            this.lblLegacyGaScalerCountCaption.Location = new System.Drawing.Point(14, 95);
            this.lblLegacyGaScalerCountCaption.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLegacyGaScalerCountCaption.Name = "lblLegacyGaScalerCountCaption";
            this.lblLegacyGaScalerCountCaption.Size = new System.Drawing.Size(97, 13);
            this.lblLegacyGaScalerCountCaption.TabIndex = 76;
            this.lblLegacyGaScalerCountCaption.Text = "Number of Scalers:";
            // 
            // lblLegacyGaLoaderCountCaption
            // 
            this.lblLegacyGaLoaderCountCaption.AutoSize = true;
            this.lblLegacyGaLoaderCountCaption.Location = new System.Drawing.Point(14, 62);
            this.lblLegacyGaLoaderCountCaption.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLegacyGaLoaderCountCaption.Name = "lblLegacyGaLoaderCountCaption";
            this.lblLegacyGaLoaderCountCaption.Size = new System.Drawing.Size(100, 13);
            this.lblLegacyGaLoaderCountCaption.TabIndex = 75;
            this.lblLegacyGaLoaderCountCaption.Text = "Number of Loaders:";
            // 
            // lblLegacyGaTruckCountCaption
            // 
            this.lblLegacyGaTruckCountCaption.AutoSize = true;
            this.lblLegacyGaTruckCountCaption.Location = new System.Drawing.Point(14, 28);
            this.lblLegacyGaTruckCountCaption.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLegacyGaTruckCountCaption.Name = "lblLegacyGaTruckCountCaption";
            this.lblLegacyGaTruckCountCaption.Size = new System.Drawing.Size(95, 13);
            this.lblLegacyGaTruckCountCaption.TabIndex = 74;
            this.lblLegacyGaTruckCountCaption.Text = "Number of Trucks:";
            // 
            // lblLegacyGaLoaderCount
            // 
            this.lblLegacyGaLoaderCount.AutoSize = true;
            this.lblLegacyGaLoaderCount.Location = new System.Drawing.Point(126, 62);
            this.lblLegacyGaLoaderCount.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLegacyGaLoaderCount.Name = "lblLegacyGaLoaderCount";
            this.lblLegacyGaLoaderCount.Size = new System.Drawing.Size(61, 13);
            this.lblLegacyGaLoaderCount.TabIndex = 73;
            this.lblLegacyGaLoaderCount.Text = "..................";
            // 
            // lblLegacyGaTruckCount
            // 
            this.lblLegacyGaTruckCount.AutoSize = true;
            this.lblLegacyGaTruckCount.Location = new System.Drawing.Point(126, 28);
            this.lblLegacyGaTruckCount.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLegacyGaTruckCount.Name = "lblLegacyGaTruckCount";
            this.lblLegacyGaTruckCount.Size = new System.Drawing.Size(61, 13);
            this.lblLegacyGaTruckCount.TabIndex = 72;
            this.lblLegacyGaTruckCount.Text = "..................";
            // 
            // btnRunGeneticAlgorithm
            // 
            this.btnRunGeneticAlgorithm.Location = new System.Drawing.Point(224, 458);
            this.btnRunGeneticAlgorithm.Name = "btnRunGeneticAlgorithm";
            this.btnRunGeneticAlgorithm.Size = new System.Drawing.Size(166, 55);
            this.btnRunGeneticAlgorithm.TabIndex = 14;
            this.btnRunGeneticAlgorithm.Text = "Run Genetic Algorithm";
            this.btnRunGeneticAlgorithm.UseVisualStyleBackColor = true;
            this.btnRunGeneticAlgorithm.Click += new System.EventHandler(this.btnRunGeneticAlgorithm_Click);
            // 
            // lblGaProgressCaption
            // 
            this.lblGaProgressCaption.AutoSize = true;
            this.lblGaProgressCaption.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblGaProgressCaption.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblGaProgressCaption.Location = new System.Drawing.Point(423, 289);
            this.lblGaProgressCaption.Name = "lblGaProgressCaption";
            this.lblGaProgressCaption.Size = new System.Drawing.Size(99, 13);
            this.lblGaProgressCaption.TabIndex = 64;
            this.lblGaProgressCaption.Text = "Simulation Progress";
            // 
            // progressGaRun
            // 
            this.progressGaRun.Location = new System.Drawing.Point(524, 289);
            this.progressGaRun.Name = "progressGaRun";
            this.progressGaRun.Size = new System.Drawing.Size(314, 23);
            this.progressGaRun.TabIndex = 63;
            // 
            // grpGaInputs
            // 
            this.grpGaInputs.Controls.Add(this.btnClearGaFields);
            this.grpGaInputs.Controls.Add(this.lblGaMaterialVolume);
            this.grpGaInputs.Controls.Add(this.lblGaLoaderCostPerDay);
            this.grpGaInputs.Controls.Add(this.lblGaScalerCostPerDay);
            this.grpGaInputs.Controls.Add(this.txtGaGenerations);
            this.grpGaInputs.Controls.Add(this.lblGaTruckCostPerDay);
            this.grpGaInputs.Controls.Add(this.lblGaPopulation);
            this.grpGaInputs.Controls.Add(this.lblGaLoadPerTruck);
            this.grpGaInputs.Controls.Add(this.txtGaMutationRate);
            this.grpGaInputs.Controls.Add(this.lblGaProjectDuration);
            this.grpGaInputs.Controls.Add(this.lblGaGenerations);
            this.grpGaInputs.Controls.Add(this.lblGaDelayCostPerDay);
            this.grpGaInputs.Controls.Add(this.txtGaPopulation);
            this.grpGaInputs.Controls.Add(this.lblGaMutationRate);
            this.grpGaInputs.Controls.Add(this.txtGaMaterialVolume);
            this.grpGaInputs.Controls.Add(this.txtGaLoadPerTruck);
            this.grpGaInputs.Controls.Add(this.txtGaMaxScalers);
            this.grpGaInputs.Controls.Add(this.txtGaTruckCostPerDay);
            this.grpGaInputs.Controls.Add(this.lblGaMaxScalers);
            this.grpGaInputs.Controls.Add(this.txtGaLoaderCostPerDay);
            this.grpGaInputs.Controls.Add(this.txtGaMaxLoaders);
            this.grpGaInputs.Controls.Add(this.txtGaScalerCostPerDay);
            this.grpGaInputs.Controls.Add(this.lblGaMaxLoaders);
            this.grpGaInputs.Controls.Add(this.txtGaProjectDuration);
            this.grpGaInputs.Controls.Add(this.txtGaMaxTrucks);
            this.grpGaInputs.Controls.Add(this.txtGaDelayCostPerDay);
            this.grpGaInputs.Controls.Add(this.lblGaMaxTrucks);
            this.grpGaInputs.Location = new System.Drawing.Point(6, 191);
            this.grpGaInputs.Name = "grpGaInputs";
            this.grpGaInputs.Size = new System.Drawing.Size(407, 300);
            this.grpGaInputs.TabIndex = 62;
            this.grpGaInputs.TabStop = false;
            this.grpGaInputs.Text = "Inputs";
            // 
            // btnClearGaFields
            // 
            this.btnClearGaFields.Location = new System.Drawing.Point(220, 252);
            this.btnClearGaFields.Name = "btnClearGaFields";
            this.btnClearGaFields.Size = new System.Drawing.Size(182, 33);
            this.btnClearGaFields.TabIndex = 15;
            this.btnClearGaFields.Text = "Clear Input";
            this.btnClearGaFields.UseVisualStyleBackColor = true;
            this.btnClearGaFields.Click += new System.EventHandler(this.btnClearGaFields_Click);
            // 
            // lblGaMaterialVolume
            // 
            this.lblGaMaterialVolume.AutoSize = true;
            this.lblGaMaterialVolume.Location = new System.Drawing.Point(7, 26);
            this.lblGaMaterialVolume.Name = "lblGaMaterialVolume";
            this.lblGaMaterialVolume.Size = new System.Drawing.Size(95, 13);
            this.lblGaMaterialVolume.TabIndex = 33;
            this.lblGaMaterialVolume.Text = "Amount of Material";
            // 
            // lblGaLoaderCostPerDay
            // 
            this.lblGaLoaderCostPerDay.AutoSize = true;
            this.lblGaLoaderCostPerDay.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblGaLoaderCostPerDay.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblGaLoaderCostPerDay.Location = new System.Drawing.Point(7, 120);
            this.lblGaLoaderCostPerDay.Name = "lblGaLoaderCostPerDay";
            this.lblGaLoaderCostPerDay.Size = new System.Drawing.Size(82, 13);
            this.lblGaLoaderCostPerDay.TabIndex = 37;
            this.lblGaLoaderCostPerDay.Text = "Cost per Loader";
            // 
            // lblGaScalerCostPerDay
            // 
            this.lblGaScalerCostPerDay.AutoSize = true;
            this.lblGaScalerCostPerDay.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblGaScalerCostPerDay.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblGaScalerCostPerDay.Location = new System.Drawing.Point(7, 151);
            this.lblGaScalerCostPerDay.Name = "lblGaScalerCostPerDay";
            this.lblGaScalerCostPerDay.Size = new System.Drawing.Size(79, 13);
            this.lblGaScalerCostPerDay.TabIndex = 36;
            this.lblGaScalerCostPerDay.Text = "Cost per Scaler";
            // 
            // txtGaGenerations
            // 
            this.txtGaGenerations.Location = new System.Drawing.Point(307, 145);
            this.txtGaGenerations.Name = "txtGaGenerations";
            this.txtGaGenerations.Size = new System.Drawing.Size(86, 20);
            this.txtGaGenerations.TabIndex = 12;
            // 
            // lblGaTruckCostPerDay
            // 
            this.lblGaTruckCostPerDay.AutoSize = true;
            this.lblGaTruckCostPerDay.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblGaTruckCostPerDay.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblGaTruckCostPerDay.Location = new System.Drawing.Point(7, 89);
            this.lblGaTruckCostPerDay.Name = "lblGaTruckCostPerDay";
            this.lblGaTruckCostPerDay.Size = new System.Drawing.Size(77, 13);
            this.lblGaTruckCostPerDay.TabIndex = 35;
            this.lblGaTruckCostPerDay.Text = "Cost per Truck";
            // 
            // lblGaPopulation
            // 
            this.lblGaPopulation.AutoSize = true;
            this.lblGaPopulation.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblGaPopulation.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblGaPopulation.Location = new System.Drawing.Point(208, 119);
            this.lblGaPopulation.Name = "lblGaPopulation";
            this.lblGaPopulation.Size = new System.Drawing.Size(57, 13);
            this.lblGaPopulation.TabIndex = 57;
            this.lblGaPopulation.Text = "Population";
            // 
            // lblGaLoadPerTruck
            // 
            this.lblGaLoadPerTruck.AutoSize = true;
            this.lblGaLoadPerTruck.Location = new System.Drawing.Point(7, 58);
            this.lblGaLoadPerTruck.Name = "lblGaLoadPerTruck";
            this.lblGaLoadPerTruck.Size = new System.Drawing.Size(81, 13);
            this.lblGaLoadPerTruck.TabIndex = 34;
            this.lblGaLoadPerTruck.Text = "Load Per Truck";
            // 
            // txtGaMutationRate
            // 
            this.txtGaMutationRate.Location = new System.Drawing.Point(306, 176);
            this.txtGaMutationRate.Name = "txtGaMutationRate";
            this.txtGaMutationRate.Size = new System.Drawing.Size(86, 20);
            this.txtGaMutationRate.TabIndex = 13;
            // 
            // lblGaProjectDuration
            // 
            this.lblGaProjectDuration.AutoSize = true;
            this.lblGaProjectDuration.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblGaProjectDuration.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblGaProjectDuration.Location = new System.Drawing.Point(7, 182);
            this.lblGaProjectDuration.Name = "lblGaProjectDuration";
            this.lblGaProjectDuration.Size = new System.Drawing.Size(83, 13);
            this.lblGaProjectDuration.TabIndex = 39;
            this.lblGaProjectDuration.Text = "Project Duration";
            // 
            // lblGaGenerations
            // 
            this.lblGaGenerations.AutoSize = true;
            this.lblGaGenerations.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblGaGenerations.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblGaGenerations.Location = new System.Drawing.Point(208, 148);
            this.lblGaGenerations.Name = "lblGaGenerations";
            this.lblGaGenerations.Size = new System.Drawing.Size(82, 13);
            this.lblGaGenerations.TabIndex = 55;
            this.lblGaGenerations.Text = "Last Generation";
            // 
            // lblGaDelayCostPerDay
            // 
            this.lblGaDelayCostPerDay.AutoSize = true;
            this.lblGaDelayCostPerDay.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblGaDelayCostPerDay.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblGaDelayCostPerDay.Location = new System.Drawing.Point(7, 214);
            this.lblGaDelayCostPerDay.Name = "lblGaDelayCostPerDay";
            this.lblGaDelayCostPerDay.Size = new System.Drawing.Size(70, 13);
            this.lblGaDelayCostPerDay.TabIndex = 38;
            this.lblGaDelayCostPerDay.Text = "Cost of Delay";
            // 
            // txtGaPopulation
            // 
            this.txtGaPopulation.Location = new System.Drawing.Point(306, 116);
            this.txtGaPopulation.Name = "txtGaPopulation";
            this.txtGaPopulation.Size = new System.Drawing.Size(86, 20);
            this.txtGaPopulation.TabIndex = 11;
            // 
            // lblGaMutationRate
            // 
            this.lblGaMutationRate.AutoSize = true;
            this.lblGaMutationRate.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblGaMutationRate.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblGaMutationRate.Location = new System.Drawing.Point(208, 179);
            this.lblGaMutationRate.Name = "lblGaMutationRate";
            this.lblGaMutationRate.Size = new System.Drawing.Size(74, 13);
            this.lblGaMutationRate.TabIndex = 53;
            this.lblGaMutationRate.Text = "Mutation Rate";
            // 
            // txtGaMaterialVolume
            // 
            this.txtGaMaterialVolume.Location = new System.Drawing.Point(105, 24);
            this.txtGaMaterialVolume.Name = "txtGaMaterialVolume";
            this.txtGaMaterialVolume.Size = new System.Drawing.Size(86, 20);
            this.txtGaMaterialVolume.TabIndex = 1;
            // 
            // txtGaLoadPerTruck
            // 
            this.txtGaLoadPerTruck.Location = new System.Drawing.Point(105, 55);
            this.txtGaLoadPerTruck.Name = "txtGaLoadPerTruck";
            this.txtGaLoadPerTruck.Size = new System.Drawing.Size(86, 20);
            this.txtGaLoadPerTruck.TabIndex = 2;
            // 
            // txtGaMaxScalers
            // 
            this.txtGaMaxScalers.Location = new System.Drawing.Point(306, 86);
            this.txtGaMaxScalers.Name = "txtGaMaxScalers";
            this.txtGaMaxScalers.Size = new System.Drawing.Size(86, 20);
            this.txtGaMaxScalers.TabIndex = 10;
            // 
            // txtGaTruckCostPerDay
            // 
            this.txtGaTruckCostPerDay.Location = new System.Drawing.Point(105, 86);
            this.txtGaTruckCostPerDay.Name = "txtGaTruckCostPerDay";
            this.txtGaTruckCostPerDay.Size = new System.Drawing.Size(86, 20);
            this.txtGaTruckCostPerDay.TabIndex = 3;
            // 
            // lblGaMaxScalers
            // 
            this.lblGaMaxScalers.AutoSize = true;
            this.lblGaMaxScalers.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblGaMaxScalers.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblGaMaxScalers.Location = new System.Drawing.Point(208, 88);
            this.lblGaMaxScalers.Name = "lblGaMaxScalers";
            this.lblGaMaxScalers.Size = new System.Drawing.Size(94, 13);
            this.lblGaMaxScalers.TabIndex = 51;
            this.lblGaMaxScalers.Text = "Number of Scalers";
            // 
            // txtGaLoaderCostPerDay
            // 
            this.txtGaLoaderCostPerDay.Location = new System.Drawing.Point(105, 117);
            this.txtGaLoaderCostPerDay.Name = "txtGaLoaderCostPerDay";
            this.txtGaLoaderCostPerDay.Size = new System.Drawing.Size(86, 20);
            this.txtGaLoaderCostPerDay.TabIndex = 4;
            // 
            // txtGaMaxLoaders
            // 
            this.txtGaMaxLoaders.Location = new System.Drawing.Point(306, 54);
            this.txtGaMaxLoaders.Name = "txtGaMaxLoaders";
            this.txtGaMaxLoaders.Size = new System.Drawing.Size(86, 20);
            this.txtGaMaxLoaders.TabIndex = 9;
            // 
            // txtGaScalerCostPerDay
            // 
            this.txtGaScalerCostPerDay.Location = new System.Drawing.Point(105, 148);
            this.txtGaScalerCostPerDay.Name = "txtGaScalerCostPerDay";
            this.txtGaScalerCostPerDay.Size = new System.Drawing.Size(86, 20);
            this.txtGaScalerCostPerDay.TabIndex = 5;
            // 
            // lblGaMaxLoaders
            // 
            this.lblGaMaxLoaders.AutoSize = true;
            this.lblGaMaxLoaders.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblGaMaxLoaders.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblGaMaxLoaders.Location = new System.Drawing.Point(208, 57);
            this.lblGaMaxLoaders.Name = "lblGaMaxLoaders";
            this.lblGaMaxLoaders.Size = new System.Drawing.Size(97, 13);
            this.lblGaMaxLoaders.TabIndex = 49;
            this.lblGaMaxLoaders.Text = "Number of Loaders";
            // 
            // txtGaProjectDuration
            // 
            this.txtGaProjectDuration.Location = new System.Drawing.Point(105, 179);
            this.txtGaProjectDuration.Name = "txtGaProjectDuration";
            this.txtGaProjectDuration.Size = new System.Drawing.Size(86, 20);
            this.txtGaProjectDuration.TabIndex = 6;
            // 
            // txtGaMaxTrucks
            // 
            this.txtGaMaxTrucks.Location = new System.Drawing.Point(306, 23);
            this.txtGaMaxTrucks.Name = "txtGaMaxTrucks";
            this.txtGaMaxTrucks.Size = new System.Drawing.Size(86, 20);
            this.txtGaMaxTrucks.TabIndex = 8;
            // 
            // txtGaDelayCostPerDay
            // 
            this.txtGaDelayCostPerDay.Location = new System.Drawing.Point(105, 211);
            this.txtGaDelayCostPerDay.Name = "txtGaDelayCostPerDay";
            this.txtGaDelayCostPerDay.Size = new System.Drawing.Size(86, 20);
            this.txtGaDelayCostPerDay.TabIndex = 7;
            // 
            // lblGaMaxTrucks
            // 
            this.lblGaMaxTrucks.AutoSize = true;
            this.lblGaMaxTrucks.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblGaMaxTrucks.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblGaMaxTrucks.Location = new System.Drawing.Point(208, 26);
            this.lblGaMaxTrucks.Name = "lblGaMaxTrucks";
            this.lblGaMaxTrucks.Size = new System.Drawing.Size(92, 13);
            this.lblGaMaxTrucks.TabIndex = 47;
            this.lblGaMaxTrucks.Text = "Number of Trucks";
            // 
            // btnRunGaDemo
            // 
            this.btnRunGaDemo.Location = new System.Drawing.Point(39, 458);
            this.btnRunGaDemo.Name = "btnRunGaDemo";
            this.btnRunGaDemo.Size = new System.Drawing.Size(166, 55);
            this.btnRunGaDemo.TabIndex = 61;
            this.btnRunGaDemo.Text = "Run Demo Using Default Values";
            this.btnRunGaDemo.UseVisualStyleBackColor = true;
            this.btnRunGaDemo.Click += new System.EventHandler(this.btnRunGaDemo_Click);
            // 
            // chartGaFitness
            // 
            this.chartGaFitness.CausesValidation = false;
            chartArea1.Name = "ChartArea1";
            this.chartGaFitness.ChartAreas.Add(chartArea1);
            legend1.Enabled = false;
            legend1.Name = "Legend1";
            this.chartGaFitness.Legends.Add(legend1);
            this.chartGaFitness.Location = new System.Drawing.Point(442, 6);
            this.chartGaFitness.Name = "chartGaFitness";
            this.chartGaFitness.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.SemiTransparent;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "BestThisGeneration";
            this.chartGaFitness.Series.Add(series1);
            this.chartGaFitness.Size = new System.Drawing.Size(412, 277);
            this.chartGaFitness.TabIndex = 60;
            this.chartGaFitness.Text = "Plot of Fitness vs Generation Number";
            title1.Name = "Title1";
            title1.Text = "Best Fitness in Each Generation";
            title2.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Left;
            title2.Name = "Title2";
            title2.Text = "Fitness";
            title3.Alignment = System.Drawing.ContentAlignment.TopCenter;
            title3.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            title3.Name = "Title3";
            title3.Text = "Generation";
            this.chartGaFitness.Titles.Add(title1);
            this.chartGaFitness.Titles.Add(title2);
            this.chartGaFitness.Titles.Add(title3);
            // 
            // picGaHeader
            // 
            this.picGaHeader.Image = global::GeneticAlgorithm.Desktop.Properties.Resources.forward_sim_gif;
            this.picGaHeader.Location = new System.Drawing.Point(6, 6);
            this.picGaHeader.Name = "picGaHeader";
            this.picGaHeader.Size = new System.Drawing.Size(407, 179);
            this.picGaHeader.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picGaHeader.TabIndex = 32;
            this.picGaHeader.TabStop = false;
            // 
            // tabSimulation
            // 
            this.tabSimulation.Controls.Add(this.chartSimulationCosts);
            this.tabSimulation.Controls.Add(this.btnClearSimulationFields);
            this.tabSimulation.Controls.Add(this.grpSimulationCostOutputs);
            this.tabSimulation.Controls.Add(this.grpSimulationUtilizationOutputs);
            this.tabSimulation.Controls.Add(this.grpSimulationInputs);
            this.tabSimulation.Controls.Add(this.btnRunSimulation);
            this.tabSimulation.Controls.Add(this.picSimulationHeader);
            this.tabSimulation.Location = new System.Drawing.Point(4, 22);
            this.tabSimulation.Name = "tabSimulation";
            this.tabSimulation.Padding = new System.Windows.Forms.Padding(3);
            this.tabSimulation.Size = new System.Drawing.Size(1312, 752);
            this.tabSimulation.TabIndex = 2;
            this.tabSimulation.Text = "Simulation";
            this.tabSimulation.UseVisualStyleBackColor = true;
            // 
            // chartSimulationCosts
            // 
            chartArea2.Name = "ChartArea1";
            this.chartSimulationCosts.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chartSimulationCosts.Legends.Add(legend2);
            this.chartSimulationCosts.Location = new System.Drawing.Point(431, 4);
            this.chartSimulationCosts.Margin = new System.Windows.Forms.Padding(2);
            this.chartSimulationCosts.Name = "chartSimulationCosts";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Actual Cost";
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Delay Cost";
            this.chartSimulationCosts.Series.Add(series2);
            this.chartSimulationCosts.Series.Add(series3);
            this.chartSimulationCosts.Size = new System.Drawing.Size(441, 317);
            this.chartSimulationCosts.TabIndex = 110;
            this.chartSimulationCosts.Text = "chartSimulationCosts";
            // 
            // btnClearSimulationFields
            // 
            this.btnClearSimulationFields.Location = new System.Drawing.Point(226, 422);
            this.btnClearSimulationFields.Name = "btnClearSimulationFields";
            this.btnClearSimulationFields.Size = new System.Drawing.Size(142, 36);
            this.btnClearSimulationFields.TabIndex = 109;
            this.btnClearSimulationFields.Text = "Clear Fields";
            this.btnClearSimulationFields.UseVisualStyleBackColor = true;
            this.btnClearSimulationFields.Click += new System.EventHandler(this.btnClearSimulationFields_Click);
            // 
            // grpSimulationCostOutputs
            // 
            this.grpSimulationCostOutputs.Controls.Add(this.lblSimDaysDelayedValue);
            this.grpSimulationCostOutputs.Controls.Add(this.lblSimDaysDelayedCaption);
            this.grpSimulationCostOutputs.Controls.Add(this.lblSimProjectDurationCaption);
            this.grpSimulationCostOutputs.Controls.Add(this.lblSimLoaderCostCaption);
            this.grpSimulationCostOutputs.Controls.Add(this.lblSimScalerCostCaption);
            this.grpSimulationCostOutputs.Controls.Add(this.lblSimTotalCostValue);
            this.grpSimulationCostOutputs.Controls.Add(this.lblSimDelayCostCaption);
            this.grpSimulationCostOutputs.Controls.Add(this.lblSimTotalCostCaption);
            this.grpSimulationCostOutputs.Controls.Add(this.lblSimTruckCostCaption);
            this.grpSimulationCostOutputs.Controls.Add(this.lblSimLoaderCostValue);
            this.grpSimulationCostOutputs.Controls.Add(this.lblSimTruckCostValue);
            this.grpSimulationCostOutputs.Controls.Add(this.lblSimDelayCostValue);
            this.grpSimulationCostOutputs.Controls.Add(this.lblSimScalerCostValue);
            this.grpSimulationCostOutputs.Controls.Add(this.lblSimProjectDurationValue);
            this.grpSimulationCostOutputs.Location = new System.Drawing.Point(607, 326);
            this.grpSimulationCostOutputs.Margin = new System.Windows.Forms.Padding(2);
            this.grpSimulationCostOutputs.Name = "grpSimulationCostOutputs";
            this.grpSimulationCostOutputs.Padding = new System.Windows.Forms.Padding(2);
            this.grpSimulationCostOutputs.Size = new System.Drawing.Size(266, 177);
            this.grpSimulationCostOutputs.TabIndex = 108;
            this.grpSimulationCostOutputs.TabStop = false;
            this.grpSimulationCostOutputs.Text = "Simulation Outputs";
            // 
            // lblSimDaysDelayedValue
            // 
            this.lblSimDaysDelayedValue.AutoSize = true;
            this.lblSimDaysDelayedValue.Location = new System.Drawing.Point(225, 26);
            this.lblSimDaysDelayedValue.Name = "lblSimDaysDelayedValue";
            this.lblSimDaysDelayedValue.Size = new System.Drawing.Size(25, 13);
            this.lblSimDaysDelayedValue.TabIndex = 107;
            this.lblSimDaysDelayedValue.Text = "......";
            // 
            // lblSimDaysDelayedCaption
            // 
            this.lblSimDaysDelayedCaption.AutoSize = true;
            this.lblSimDaysDelayedCaption.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblSimDaysDelayedCaption.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblSimDaysDelayedCaption.Location = new System.Drawing.Point(144, 26);
            this.lblSimDaysDelayedCaption.Name = "lblSimDaysDelayedCaption";
            this.lblSimDaysDelayedCaption.Size = new System.Drawing.Size(76, 13);
            this.lblSimDaysDelayedCaption.TabIndex = 106;
            this.lblSimDaysDelayedCaption.Text = "Days Delayed:";
            // 
            // lblSimProjectDurationCaption
            // 
            this.lblSimProjectDurationCaption.AutoSize = true;
            this.lblSimProjectDurationCaption.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblSimProjectDurationCaption.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblSimProjectDurationCaption.Location = new System.Drawing.Point(5, 26);
            this.lblSimProjectDurationCaption.Name = "lblSimProjectDurationCaption";
            this.lblSimProjectDurationCaption.Size = new System.Drawing.Size(86, 13);
            this.lblSimProjectDurationCaption.TabIndex = 86;
            this.lblSimProjectDurationCaption.Text = "Project Duration:";
            // 
            // lblSimLoaderCostCaption
            // 
            this.lblSimLoaderCostCaption.AutoSize = true;
            this.lblSimLoaderCostCaption.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblSimLoaderCostCaption.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblSimLoaderCostCaption.Location = new System.Drawing.Point(5, 87);
            this.lblSimLoaderCostCaption.Name = "lblSimLoaderCostCaption";
            this.lblSimLoaderCostCaption.Size = new System.Drawing.Size(72, 13);
            this.lblSimLoaderCostCaption.TabIndex = 84;
            this.lblSimLoaderCostCaption.Text = "Loaders Cost:";
            // 
            // lblSimScalerCostCaption
            // 
            this.lblSimScalerCostCaption.AutoSize = true;
            this.lblSimScalerCostCaption.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblSimScalerCostCaption.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblSimScalerCostCaption.Location = new System.Drawing.Point(9, 119);
            this.lblSimScalerCostCaption.Name = "lblSimScalerCostCaption";
            this.lblSimScalerCostCaption.Size = new System.Drawing.Size(69, 13);
            this.lblSimScalerCostCaption.TabIndex = 83;
            this.lblSimScalerCostCaption.Text = "Scalers Cost:";
            // 
            // lblSimTotalCostValue
            // 
            this.lblSimTotalCostValue.AutoSize = true;
            this.lblSimTotalCostValue.Location = new System.Drawing.Point(225, 93);
            this.lblSimTotalCostValue.Name = "lblSimTotalCostValue";
            this.lblSimTotalCostValue.Size = new System.Drawing.Size(25, 13);
            this.lblSimTotalCostValue.TabIndex = 105;
            this.lblSimTotalCostValue.Text = "......";
            // 
            // lblSimDelayCostCaption
            // 
            this.lblSimDelayCostCaption.AutoSize = true;
            this.lblSimDelayCostCaption.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblSimDelayCostCaption.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblSimDelayCostCaption.Location = new System.Drawing.Point(159, 59);
            this.lblSimDelayCostCaption.Name = "lblSimDelayCostCaption";
            this.lblSimDelayCostCaption.Size = new System.Drawing.Size(61, 13);
            this.lblSimDelayCostCaption.TabIndex = 85;
            this.lblSimDelayCostCaption.Text = "Delay Cost:";
            // 
            // lblSimTotalCostCaption
            // 
            this.lblSimTotalCostCaption.AutoSize = true;
            this.lblSimTotalCostCaption.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblSimTotalCostCaption.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblSimTotalCostCaption.Location = new System.Drawing.Point(159, 93);
            this.lblSimTotalCostCaption.Name = "lblSimTotalCostCaption";
            this.lblSimTotalCostCaption.Size = new System.Drawing.Size(58, 13);
            this.lblSimTotalCostCaption.TabIndex = 104;
            this.lblSimTotalCostCaption.Text = "Total Cost:";
            // 
            // lblSimTruckCostCaption
            // 
            this.lblSimTruckCostCaption.AutoSize = true;
            this.lblSimTruckCostCaption.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblSimTruckCostCaption.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblSimTruckCostCaption.Location = new System.Drawing.Point(9, 56);
            this.lblSimTruckCostCaption.Name = "lblSimTruckCostCaption";
            this.lblSimTruckCostCaption.Size = new System.Drawing.Size(67, 13);
            this.lblSimTruckCostCaption.TabIndex = 94;
            this.lblSimTruckCostCaption.Text = "Trucks Cost:";
            // 
            // lblSimLoaderCostValue
            // 
            this.lblSimLoaderCostValue.AutoSize = true;
            this.lblSimLoaderCostValue.Location = new System.Drawing.Point(104, 87);
            this.lblSimLoaderCostValue.Name = "lblSimLoaderCostValue";
            this.lblSimLoaderCostValue.Size = new System.Drawing.Size(25, 13);
            this.lblSimLoaderCostValue.TabIndex = 98;
            this.lblSimLoaderCostValue.Text = "......";
            // 
            // lblSimTruckCostValue
            // 
            this.lblSimTruckCostValue.AutoSize = true;
            this.lblSimTruckCostValue.Location = new System.Drawing.Point(104, 56);
            this.lblSimTruckCostValue.Name = "lblSimTruckCostValue";
            this.lblSimTruckCostValue.Size = new System.Drawing.Size(25, 13);
            this.lblSimTruckCostValue.TabIndex = 102;
            this.lblSimTruckCostValue.Text = "......";
            // 
            // lblSimDelayCostValue
            // 
            this.lblSimDelayCostValue.AutoSize = true;
            this.lblSimDelayCostValue.Location = new System.Drawing.Point(225, 59);
            this.lblSimDelayCostValue.Name = "lblSimDelayCostValue";
            this.lblSimDelayCostValue.Size = new System.Drawing.Size(25, 13);
            this.lblSimDelayCostValue.TabIndex = 101;
            this.lblSimDelayCostValue.Text = "......";
            // 
            // lblSimScalerCostValue
            // 
            this.lblSimScalerCostValue.AutoSize = true;
            this.lblSimScalerCostValue.Location = new System.Drawing.Point(104, 119);
            this.lblSimScalerCostValue.Name = "lblSimScalerCostValue";
            this.lblSimScalerCostValue.Size = new System.Drawing.Size(25, 13);
            this.lblSimScalerCostValue.TabIndex = 99;
            this.lblSimScalerCostValue.Text = "......";
            // 
            // lblSimProjectDurationValue
            // 
            this.lblSimProjectDurationValue.AutoSize = true;
            this.lblSimProjectDurationValue.Location = new System.Drawing.Point(104, 26);
            this.lblSimProjectDurationValue.Name = "lblSimProjectDurationValue";
            this.lblSimProjectDurationValue.Size = new System.Drawing.Size(25, 13);
            this.lblSimProjectDurationValue.TabIndex = 100;
            this.lblSimProjectDurationValue.Text = "......";
            // 
            // grpSimulationUtilizationOutputs
            // 
            this.grpSimulationUtilizationOutputs.Controls.Add(this.lblSimUtilTrucksCaption);
            this.grpSimulationUtilizationOutputs.Controls.Add(this.lblSimUtilScalersCaption);
            this.grpSimulationUtilizationOutputs.Controls.Add(this.lblSimUtilLoadersCaption);
            this.grpSimulationUtilizationOutputs.Controls.Add(this.lblSimUtilTrucksValue);
            this.grpSimulationUtilizationOutputs.Controls.Add(this.lblSimUtilLoadersValue);
            this.grpSimulationUtilizationOutputs.Controls.Add(this.lblSimUtilScalersValue);
            this.grpSimulationUtilizationOutputs.Location = new System.Drawing.Point(424, 326);
            this.grpSimulationUtilizationOutputs.Margin = new System.Windows.Forms.Padding(2);
            this.grpSimulationUtilizationOutputs.Name = "grpSimulationUtilizationOutputs";
            this.grpSimulationUtilizationOutputs.Padding = new System.Windows.Forms.Padding(2);
            this.grpSimulationUtilizationOutputs.Size = new System.Drawing.Size(178, 177);
            this.grpSimulationUtilizationOutputs.TabIndex = 107;
            this.grpSimulationUtilizationOutputs.TabStop = false;
            this.grpSimulationUtilizationOutputs.Text = "Utilization OutPuts";
            // 
            // lblSimUtilTrucksCaption
            // 
            this.lblSimUtilTrucksCaption.AutoSize = true;
            this.lblSimUtilTrucksCaption.Location = new System.Drawing.Point(16, 28);
            this.lblSimUtilTrucksCaption.Name = "lblSimUtilTrucksCaption";
            this.lblSimUtilTrucksCaption.Size = new System.Drawing.Size(100, 13);
            this.lblSimUtilTrucksCaption.TabIndex = 80;
            this.lblSimUtilTrucksCaption.Text = "Utilization of Trucks";
            // 
            // lblSimUtilScalersCaption
            // 
            this.lblSimUtilScalersCaption.AutoSize = true;
            this.lblSimUtilScalersCaption.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblSimUtilScalersCaption.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblSimUtilScalersCaption.Location = new System.Drawing.Point(16, 90);
            this.lblSimUtilScalersCaption.Name = "lblSimUtilScalersCaption";
            this.lblSimUtilScalersCaption.Size = new System.Drawing.Size(100, 13);
            this.lblSimUtilScalersCaption.TabIndex = 82;
            this.lblSimUtilScalersCaption.Text = "Utlization of Scalers";
            // 
            // lblSimUtilLoadersCaption
            // 
            this.lblSimUtilLoadersCaption.AutoSize = true;
            this.lblSimUtilLoadersCaption.Location = new System.Drawing.Point(16, 59);
            this.lblSimUtilLoadersCaption.Name = "lblSimUtilLoadersCaption";
            this.lblSimUtilLoadersCaption.Size = new System.Drawing.Size(98, 13);
            this.lblSimUtilLoadersCaption.TabIndex = 81;
            this.lblSimUtilLoadersCaption.Text = "Utlization of Loader";
            // 
            // lblSimUtilTrucksValue
            // 
            this.lblSimUtilTrucksValue.AutoSize = true;
            this.lblSimUtilTrucksValue.Location = new System.Drawing.Point(128, 26);
            this.lblSimUtilTrucksValue.Name = "lblSimUtilTrucksValue";
            this.lblSimUtilTrucksValue.Size = new System.Drawing.Size(25, 13);
            this.lblSimUtilTrucksValue.TabIndex = 95;
            this.lblSimUtilTrucksValue.Text = "......";
            // 
            // lblSimUtilLoadersValue
            // 
            this.lblSimUtilLoadersValue.AutoSize = true;
            this.lblSimUtilLoadersValue.Location = new System.Drawing.Point(128, 56);
            this.lblSimUtilLoadersValue.Name = "lblSimUtilLoadersValue";
            this.lblSimUtilLoadersValue.Size = new System.Drawing.Size(25, 13);
            this.lblSimUtilLoadersValue.TabIndex = 96;
            this.lblSimUtilLoadersValue.Text = "......";
            // 
            // lblSimUtilScalersValue
            // 
            this.lblSimUtilScalersValue.AutoSize = true;
            this.lblSimUtilScalersValue.Location = new System.Drawing.Point(128, 87);
            this.lblSimUtilScalersValue.Name = "lblSimUtilScalersValue";
            this.lblSimUtilScalersValue.Size = new System.Drawing.Size(25, 13);
            this.lblSimUtilScalersValue.TabIndex = 97;
            this.lblSimUtilScalersValue.Text = "......";
            // 
            // grpSimulationInputs
            // 
            this.grpSimulationInputs.Controls.Add(this.txtSimScalerCount);
            this.grpSimulationInputs.Controls.Add(this.lblSimScalerCount);
            this.grpSimulationInputs.Controls.Add(this.txtSimLoaderCount);
            this.grpSimulationInputs.Controls.Add(this.lblSimLoaderCount);
            this.grpSimulationInputs.Controls.Add(this.txtSimTruckCount);
            this.grpSimulationInputs.Controls.Add(this.lblSimTruckCount);
            this.grpSimulationInputs.Controls.Add(this.txtSimLoaderCostPerDay);
            this.grpSimulationInputs.Controls.Add(this.txtSimDelayCostPerDay);
            this.grpSimulationInputs.Controls.Add(this.txtSimProjectDuration);
            this.grpSimulationInputs.Controls.Add(this.txtSimScalerCostPerDay);
            this.grpSimulationInputs.Controls.Add(this.txtSimTruckCostPerDay);
            this.grpSimulationInputs.Controls.Add(this.txtSimLoadPerTruck);
            this.grpSimulationInputs.Controls.Add(this.txtSimMaterialVolume);
            this.grpSimulationInputs.Controls.Add(this.lblSimMaterialVolume);
            this.grpSimulationInputs.Controls.Add(this.lblSimDelayCostPerDay);
            this.grpSimulationInputs.Controls.Add(this.lblSimProjectDuration);
            this.grpSimulationInputs.Controls.Add(this.lblSimLoadPerTruck);
            this.grpSimulationInputs.Controls.Add(this.lblSimTruckCostPerDay);
            this.grpSimulationInputs.Controls.Add(this.lblSimScalerCostPerDay);
            this.grpSimulationInputs.Controls.Add(this.lblSimLoaderCostPerDay);
            this.grpSimulationInputs.Location = new System.Drawing.Point(0, 204);
            this.grpSimulationInputs.Margin = new System.Windows.Forms.Padding(2);
            this.grpSimulationInputs.Name = "grpSimulationInputs";
            this.grpSimulationInputs.Padding = new System.Windows.Forms.Padding(2);
            this.grpSimulationInputs.Size = new System.Drawing.Size(420, 177);
            this.grpSimulationInputs.TabIndex = 106;
            this.grpSimulationInputs.TabStop = false;
            this.grpSimulationInputs.Text = "Simulation Inputs";
            // 
            // txtSimScalerCount
            // 
            this.txtSimScalerCount.Location = new System.Drawing.Point(302, 72);
            this.txtSimScalerCount.Name = "txtSimScalerCount";
            this.txtSimScalerCount.Size = new System.Drawing.Size(86, 20);
            this.txtSimScalerCount.TabIndex = 8;
            // 
            // lblSimScalerCount
            // 
            this.lblSimScalerCount.AutoSize = true;
            this.lblSimScalerCount.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblSimScalerCount.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblSimScalerCount.Location = new System.Drawing.Point(204, 73);
            this.lblSimScalerCount.Name = "lblSimScalerCount";
            this.lblSimScalerCount.Size = new System.Drawing.Size(94, 13);
            this.lblSimScalerCount.TabIndex = 78;
            this.lblSimScalerCount.Text = "Number of Scalers";
            // 
            // txtSimLoaderCount
            // 
            this.txtSimLoaderCount.Location = new System.Drawing.Point(302, 39);
            this.txtSimLoaderCount.Name = "txtSimLoaderCount";
            this.txtSimLoaderCount.Size = new System.Drawing.Size(86, 20);
            this.txtSimLoaderCount.TabIndex = 7;
            // 
            // lblSimLoaderCount
            // 
            this.lblSimLoaderCount.AutoSize = true;
            this.lblSimLoaderCount.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblSimLoaderCount.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblSimLoaderCount.Location = new System.Drawing.Point(204, 42);
            this.lblSimLoaderCount.Name = "lblSimLoaderCount";
            this.lblSimLoaderCount.Size = new System.Drawing.Size(97, 13);
            this.lblSimLoaderCount.TabIndex = 76;
            this.lblSimLoaderCount.Text = "Number of Loaders";
            // 
            // txtSimTruckCount
            // 
            this.txtSimTruckCount.Location = new System.Drawing.Point(302, 8);
            this.txtSimTruckCount.Name = "txtSimTruckCount";
            this.txtSimTruckCount.Size = new System.Drawing.Size(86, 20);
            this.txtSimTruckCount.TabIndex = 6;
            // 
            // lblSimTruckCount
            // 
            this.lblSimTruckCount.AutoSize = true;
            this.lblSimTruckCount.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblSimTruckCount.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblSimTruckCount.Location = new System.Drawing.Point(204, 11);
            this.lblSimTruckCount.Name = "lblSimTruckCount";
            this.lblSimTruckCount.Size = new System.Drawing.Size(92, 13);
            this.lblSimTruckCount.TabIndex = 74;
            this.lblSimTruckCount.Text = "Number of Trucks";
            // 
            // txtSimLoaderCostPerDay
            // 
            this.txtSimLoaderCostPerDay.Location = new System.Drawing.Point(101, 104);
            this.txtSimLoaderCostPerDay.Name = "txtSimLoaderCostPerDay";
            this.txtSimLoaderCostPerDay.Size = new System.Drawing.Size(86, 20);
            this.txtSimLoaderCostPerDay.TabIndex = 4;
            // 
            // txtSimDelayCostPerDay
            // 
            this.txtSimDelayCostPerDay.Location = new System.Drawing.Point(302, 133);
            this.txtSimDelayCostPerDay.Name = "txtSimDelayCostPerDay";
            this.txtSimDelayCostPerDay.Size = new System.Drawing.Size(86, 20);
            this.txtSimDelayCostPerDay.TabIndex = 10;
            // 
            // txtSimProjectDuration
            // 
            this.txtSimProjectDuration.Location = new System.Drawing.Point(302, 102);
            this.txtSimProjectDuration.Name = "txtSimProjectDuration";
            this.txtSimProjectDuration.Size = new System.Drawing.Size(86, 20);
            this.txtSimProjectDuration.TabIndex = 9;
            // 
            // txtSimScalerCostPerDay
            // 
            this.txtSimScalerCostPerDay.Location = new System.Drawing.Point(101, 136);
            this.txtSimScalerCostPerDay.Name = "txtSimScalerCostPerDay";
            this.txtSimScalerCostPerDay.Size = new System.Drawing.Size(86, 20);
            this.txtSimScalerCostPerDay.TabIndex = 5;
            // 
            // txtSimTruckCostPerDay
            // 
            this.txtSimTruckCostPerDay.Location = new System.Drawing.Point(101, 73);
            this.txtSimTruckCostPerDay.Name = "txtSimTruckCostPerDay";
            this.txtSimTruckCostPerDay.Size = new System.Drawing.Size(86, 20);
            this.txtSimTruckCostPerDay.TabIndex = 3;
            // 
            // txtSimLoadPerTruck
            // 
            this.txtSimLoadPerTruck.Location = new System.Drawing.Point(101, 42);
            this.txtSimLoadPerTruck.Name = "txtSimLoadPerTruck";
            this.txtSimLoadPerTruck.Size = new System.Drawing.Size(86, 20);
            this.txtSimLoadPerTruck.TabIndex = 2;
            // 
            // txtSimMaterialVolume
            // 
            this.txtSimMaterialVolume.Location = new System.Drawing.Point(101, 11);
            this.txtSimMaterialVolume.Name = "txtSimMaterialVolume";
            this.txtSimMaterialVolume.Size = new System.Drawing.Size(86, 20);
            this.txtSimMaterialVolume.TabIndex = 1;
            // 
            // lblSimMaterialVolume
            // 
            this.lblSimMaterialVolume.AutoSize = true;
            this.lblSimMaterialVolume.Location = new System.Drawing.Point(3, 13);
            this.lblSimMaterialVolume.Name = "lblSimMaterialVolume";
            this.lblSimMaterialVolume.Size = new System.Drawing.Size(95, 13);
            this.lblSimMaterialVolume.TabIndex = 60;
            this.lblSimMaterialVolume.Text = "Amount of Material";
            // 
            // lblSimDelayCostPerDay
            // 
            this.lblSimDelayCostPerDay.AutoSize = true;
            this.lblSimDelayCostPerDay.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblSimDelayCostPerDay.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblSimDelayCostPerDay.Location = new System.Drawing.Point(204, 136);
            this.lblSimDelayCostPerDay.Name = "lblSimDelayCostPerDay";
            this.lblSimDelayCostPerDay.Size = new System.Drawing.Size(70, 13);
            this.lblSimDelayCostPerDay.TabIndex = 65;
            this.lblSimDelayCostPerDay.Text = "Cost of Delay";
            // 
            // lblSimProjectDuration
            // 
            this.lblSimProjectDuration.AutoSize = true;
            this.lblSimProjectDuration.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblSimProjectDuration.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblSimProjectDuration.Location = new System.Drawing.Point(204, 104);
            this.lblSimProjectDuration.Name = "lblSimProjectDuration";
            this.lblSimProjectDuration.Size = new System.Drawing.Size(83, 13);
            this.lblSimProjectDuration.TabIndex = 66;
            this.lblSimProjectDuration.Text = "Project Duration";
            // 
            // lblSimLoadPerTruck
            // 
            this.lblSimLoadPerTruck.AutoSize = true;
            this.lblSimLoadPerTruck.Location = new System.Drawing.Point(3, 45);
            this.lblSimLoadPerTruck.Name = "lblSimLoadPerTruck";
            this.lblSimLoadPerTruck.Size = new System.Drawing.Size(81, 13);
            this.lblSimLoadPerTruck.TabIndex = 61;
            this.lblSimLoadPerTruck.Text = "Load Per Truck";
            // 
            // lblSimTruckCostPerDay
            // 
            this.lblSimTruckCostPerDay.AutoSize = true;
            this.lblSimTruckCostPerDay.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblSimTruckCostPerDay.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblSimTruckCostPerDay.Location = new System.Drawing.Point(3, 76);
            this.lblSimTruckCostPerDay.Name = "lblSimTruckCostPerDay";
            this.lblSimTruckCostPerDay.Size = new System.Drawing.Size(77, 13);
            this.lblSimTruckCostPerDay.TabIndex = 62;
            this.lblSimTruckCostPerDay.Text = "Cost per Truck";
            // 
            // lblSimScalerCostPerDay
            // 
            this.lblSimScalerCostPerDay.AutoSize = true;
            this.lblSimScalerCostPerDay.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblSimScalerCostPerDay.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblSimScalerCostPerDay.Location = new System.Drawing.Point(3, 138);
            this.lblSimScalerCostPerDay.Name = "lblSimScalerCostPerDay";
            this.lblSimScalerCostPerDay.Size = new System.Drawing.Size(79, 13);
            this.lblSimScalerCostPerDay.TabIndex = 63;
            this.lblSimScalerCostPerDay.Text = "Cost per Scaler";
            // 
            // lblSimLoaderCostPerDay
            // 
            this.lblSimLoaderCostPerDay.AutoSize = true;
            this.lblSimLoaderCostPerDay.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblSimLoaderCostPerDay.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblSimLoaderCostPerDay.Location = new System.Drawing.Point(3, 107);
            this.lblSimLoaderCostPerDay.Name = "lblSimLoaderCostPerDay";
            this.lblSimLoaderCostPerDay.Size = new System.Drawing.Size(82, 13);
            this.lblSimLoaderCostPerDay.TabIndex = 64;
            this.lblSimLoaderCostPerDay.Text = "Cost per Loader";
            // 
            // btnRunSimulation
            // 
            this.btnRunSimulation.Location = new System.Drawing.Point(56, 422);
            this.btnRunSimulation.Name = "btnRunSimulation";
            this.btnRunSimulation.Size = new System.Drawing.Size(142, 36);
            this.btnRunSimulation.TabIndex = 11;
            this.btnRunSimulation.Text = "Run Simulation";
            this.btnRunSimulation.UseVisualStyleBackColor = true;
            this.btnRunSimulation.Click += new System.EventHandler(this.btnRunSimulation_Click);
            // 
            // picSimulationHeader
            // 
            this.picSimulationHeader.Image = global::GeneticAlgorithm.Desktop.Properties.Resources._6iC;
            this.picSimulationHeader.Location = new System.Drawing.Point(6, 6);
            this.picSimulationHeader.Name = "picSimulationHeader";
            this.picSimulationHeader.Size = new System.Drawing.Size(407, 179);
            this.picSimulationHeader.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picSimulationHeader.TabIndex = 59;
            this.picSimulationHeader.TabStop = false;
            // 
            // tabDistribution
            // 
            this.tabDistribution.Controls.Add(this.grpDistributionDefaults);
            this.tabDistribution.Controls.Add(this.grpDistributionNewValues);
            this.tabDistribution.Location = new System.Drawing.Point(4, 22);
            this.tabDistribution.Name = "tabDistribution";
            this.tabDistribution.Padding = new System.Windows.Forms.Padding(3);
            this.tabDistribution.Size = new System.Drawing.Size(876, 535);
            this.tabDistribution.TabIndex = 1;
            this.tabDistribution.Text = "Distribution";
            this.tabDistribution.UseVisualStyleBackColor = true;
            // 
            // grpDistributionDefaults
            // 
            this.grpDistributionDefaults.Controls.Add(this.picDefaultLoadingDistribution);
            this.grpDistributionDefaults.Controls.Add(this.picDefaultTravelDistribution);
            this.grpDistributionDefaults.Controls.Add(this.picDefaultWeighingDistribution);
            this.grpDistributionDefaults.Location = new System.Drawing.Point(7, 6);
            this.grpDistributionDefaults.Margin = new System.Windows.Forms.Padding(2);
            this.grpDistributionDefaults.Name = "grpDistributionDefaults";
            this.grpDistributionDefaults.Padding = new System.Windows.Forms.Padding(2);
            this.grpDistributionDefaults.Size = new System.Drawing.Size(287, 522);
            this.grpDistributionDefaults.TabIndex = 38;
            this.grpDistributionDefaults.TabStop = false;
            this.grpDistributionDefaults.Text = "Default Values";
            // 
            // picDefaultLoadingDistribution
            // 
            this.picDefaultLoadingDistribution.Image = ((System.Drawing.Image)(resources.GetObject("picDefaultLoadingDistribution.Image")));
            this.picDefaultLoadingDistribution.Location = new System.Drawing.Point(16, 32);
            this.picDefaultLoadingDistribution.Name = "picDefaultLoadingDistribution";
            this.picDefaultLoadingDistribution.Size = new System.Drawing.Size(243, 150);
            this.picDefaultLoadingDistribution.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picDefaultLoadingDistribution.TabIndex = 39;
            this.picDefaultLoadingDistribution.TabStop = false;
            // 
            // picDefaultTravelDistribution
            // 
            this.picDefaultTravelDistribution.Image = ((System.Drawing.Image)(resources.GetObject("picDefaultTravelDistribution.Image")));
            this.picDefaultTravelDistribution.Location = new System.Drawing.Point(16, 349);
            this.picDefaultTravelDistribution.Name = "picDefaultTravelDistribution";
            this.picDefaultTravelDistribution.Size = new System.Drawing.Size(243, 150);
            this.picDefaultTravelDistribution.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picDefaultTravelDistribution.TabIndex = 41;
            this.picDefaultTravelDistribution.TabStop = false;
            // 
            // picDefaultWeighingDistribution
            // 
            this.picDefaultWeighingDistribution.Image = ((System.Drawing.Image)(resources.GetObject("picDefaultWeighingDistribution.Image")));
            this.picDefaultWeighingDistribution.Location = new System.Drawing.Point(16, 189);
            this.picDefaultWeighingDistribution.Name = "picDefaultWeighingDistribution";
            this.picDefaultWeighingDistribution.Size = new System.Drawing.Size(243, 150);
            this.picDefaultWeighingDistribution.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picDefaultWeighingDistribution.TabIndex = 40;
            this.picDefaultWeighingDistribution.TabStop = false;
            // 
            // grpDistributionNewValues
            // 
            this.grpDistributionNewValues.Controls.Add(this.lblTravelingDistributionHeader);
            this.grpDistributionNewValues.Controls.Add(this.lblWeighingDistributionHeader);
            this.grpDistributionNewValues.Controls.Add(this.lblLoadingDistributionHeader);
            this.grpDistributionNewValues.Controls.Add(this.btnClearDistributionGrids);
            this.grpDistributionNewValues.Controls.Add(this.btnAddDistributionsToSimulation);
            this.grpDistributionNewValues.Controls.Add(this.gridTravelingDistribution);
            this.grpDistributionNewValues.Controls.Add(this.gridWeighingDistribution);
            this.grpDistributionNewValues.Controls.Add(this.gridLoadingDistribution);
            this.grpDistributionNewValues.Location = new System.Drawing.Point(318, 6);
            this.grpDistributionNewValues.Name = "grpDistributionNewValues";
            this.grpDistributionNewValues.Size = new System.Drawing.Size(557, 523);
            this.grpDistributionNewValues.TabIndex = 37;
            this.grpDistributionNewValues.TabStop = false;
            this.grpDistributionNewValues.Text = "New Values";
            // 
            // lblTravelingDistributionHeader
            // 
            this.lblTravelingDistributionHeader.AutoSize = true;
            this.lblTravelingDistributionHeader.Location = new System.Drawing.Point(392, 20);
            this.lblTravelingDistributionHeader.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTravelingDistributionHeader.Name = "lblTravelingDistributionHeader";
            this.lblTravelingDistributionHeader.Size = new System.Drawing.Size(128, 13);
            this.lblTravelingDistributionHeader.TabIndex = 11;
            this.lblTravelingDistributionHeader.Text = "Traveling time distribution";
            // 
            // lblWeighingDistributionHeader
            // 
            this.lblWeighingDistributionHeader.AutoSize = true;
            this.lblWeighingDistributionHeader.Location = new System.Drawing.Point(199, 20);
            this.lblWeighingDistributionHeader.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblWeighingDistributionHeader.Name = "lblWeighingDistributionHeader";
            this.lblWeighingDistributionHeader.Size = new System.Drawing.Size(123, 13);
            this.lblWeighingDistributionHeader.TabIndex = 10;
            this.lblWeighingDistributionHeader.Text = "Weighing time distribution";
            // 
            // lblLoadingDistributionHeader
            // 
            this.lblLoadingDistributionHeader.AutoSize = true;
            this.lblLoadingDistributionHeader.Location = new System.Drawing.Point(32, 20);
            this.lblLoadingDistributionHeader.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLoadingDistributionHeader.Name = "lblLoadingDistributionHeader";
            this.lblLoadingDistributionHeader.Size = new System.Drawing.Size(126, 13);
            this.lblLoadingDistributionHeader.TabIndex = 9;
            this.lblLoadingDistributionHeader.Text = "Loading time distribution";
            // 
            // btnClearDistributionGrids
            // 
            this.btnClearDistributionGrids.Location = new System.Drawing.Point(302, 422);
            this.btnClearDistributionGrids.Name = "btnClearDistributionGrids";
            this.btnClearDistributionGrids.Size = new System.Drawing.Size(166, 34);
            this.btnClearDistributionGrids.TabIndex = 8;
            this.btnClearDistributionGrids.Text = "Clear Tables";
            this.btnClearDistributionGrids.UseVisualStyleBackColor = true;
            this.btnClearDistributionGrids.Click += new System.EventHandler(this.btnClearDistributionGrids_Click);
            // 
            // btnAddDistributionsToSimulation
            // 
            this.btnAddDistributionsToSimulation.Location = new System.Drawing.Point(79, 422);
            this.btnAddDistributionsToSimulation.Name = "btnAddDistributionsToSimulation";
            this.btnAddDistributionsToSimulation.Size = new System.Drawing.Size(166, 34);
            this.btnAddDistributionsToSimulation.TabIndex = 7;
            this.btnAddDistributionsToSimulation.Text = "Add to Simulation";
            this.btnAddDistributionsToSimulation.UseVisualStyleBackColor = true;
            this.btnAddDistributionsToSimulation.Click += new System.EventHandler(this.btnAddDistributionsToSimulation_Click);
            // 
            // gridTravelingDistribution
            // 
            this.gridTravelingDistribution.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.gridTravelingDistribution.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.gridTravelingDistribution.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridTravelingDistribution.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTravelingTime,
            this.colTravelingProbability});
            this.gridTravelingDistribution.Location = new System.Drawing.Point(374, 47);
            this.gridTravelingDistribution.Name = "gridTravelingDistribution";
            this.gridTravelingDistribution.RowHeadersWidth = 51;
            this.gridTravelingDistribution.Size = new System.Drawing.Size(168, 355);
            this.gridTravelingDistribution.TabIndex = 5;
            this.gridTravelingDistribution.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridView_CellValidating);
            // 
            // colTravelingTime
            // 
            this.colTravelingTime.HeaderText = "Time (min)";
            this.colTravelingTime.MinimumWidth = 6;
            this.colTravelingTime.Name = "colTravelingTime";
            this.colTravelingTime.Width = 55;
            // 
            // colTravelingProbability
            // 
            this.colTravelingProbability.HeaderText = "Probability";
            this.colTravelingProbability.MinimumWidth = 6;
            this.colTravelingProbability.Name = "colTravelingProbability";
            this.colTravelingProbability.Width = 80;
            // 
            // gridWeighingDistribution
            // 
            this.gridWeighingDistribution.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.gridWeighingDistribution.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.gridWeighingDistribution.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridWeighingDistribution.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colWeighingTime,
            this.colWeighingProbability});
            this.gridWeighingDistribution.Location = new System.Drawing.Point(190, 47);
            this.gridWeighingDistribution.Name = "gridWeighingDistribution";
            this.gridWeighingDistribution.RowHeadersWidth = 51;
            this.gridWeighingDistribution.Size = new System.Drawing.Size(168, 355);
            this.gridWeighingDistribution.TabIndex = 4;
            this.gridWeighingDistribution.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridView_CellValidating);
            // 
            // colWeighingTime
            // 
            this.colWeighingTime.HeaderText = "Time (min)";
            this.colWeighingTime.MinimumWidth = 6;
            this.colWeighingTime.Name = "colWeighingTime";
            this.colWeighingTime.Width = 55;
            // 
            // colWeighingProbability
            // 
            this.colWeighingProbability.HeaderText = "Probability";
            this.colWeighingProbability.MinimumWidth = 6;
            this.colWeighingProbability.Name = "colWeighingProbability";
            this.colWeighingProbability.Width = 80;
            // 
            // gridLoadingDistribution
            // 
            this.gridLoadingDistribution.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.gridLoadingDistribution.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.gridLoadingDistribution.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridLoadingDistribution.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colLoadingTime,
            this.colLoadingProbability});
            this.gridLoadingDistribution.Location = new System.Drawing.Point(8, 47);
            this.gridLoadingDistribution.Name = "gridLoadingDistribution";
            this.gridLoadingDistribution.RowHeadersWidth = 51;
            this.gridLoadingDistribution.Size = new System.Drawing.Size(168, 355);
            this.gridLoadingDistribution.TabIndex = 3;
            this.gridLoadingDistribution.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridView_CellValidating);
            // 
            // colLoadingTime
            // 
            this.colLoadingTime.HeaderText = "Time (min)";
            this.colLoadingTime.MinimumWidth = 6;
            this.colLoadingTime.Name = "colLoadingTime";
            this.colLoadingTime.Width = 55;
            // 
            // colLoadingProbability
            // 
            this.colLoadingProbability.HeaderText = "Probability";
            this.colLoadingProbability.MinimumWidth = 6;
            this.colLoadingProbability.Name = "colLoadingProbability";
            this.colLoadingProbability.Width = 80;
            // 
            // mainTabControl
            // 
            this.mainTabControl.Controls.Add(this.tabDistribution);
            this.mainTabControl.Controls.Add(this.tabSimulation);
            this.mainTabControl.Controls.Add(this.tabGeneticAlgorithm);
            this.mainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTabControl.Location = new System.Drawing.Point(0, 0);
            this.mainTabControl.Name = "mainTabControl";
            this.mainTabControl.SelectedIndex = 0;
            this.mainTabControl.Size = new System.Drawing.Size(884, 561);
            this.mainTabControl.TabIndex = 33;
            // 
            // OptimizationView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1320, 780);
            this.Controls.Add(this.mainTabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = true;
            this.MinimumSize = new System.Drawing.Size(1200, 680);
            this.Name = "OptimizationView";
            this.Text = "Truck Fleet Problem";
            this.Load += new System.EventHandler(this.OptimizationView_Load);
            this.tabGeneticAlgorithm.ResumeLayout(false);
            this.tabGeneticAlgorithm.PerformLayout();
            this.grpLegacyGaOutputs.ResumeLayout(false);
            this.grpLegacyGaOutputs.PerformLayout();
            this.grpGaInputs.ResumeLayout(false);
            this.grpGaInputs.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartGaFitness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGaHeader)).EndInit();
            this.tabSimulation.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartSimulationCosts)).EndInit();
            this.grpSimulationCostOutputs.ResumeLayout(false);
            this.grpSimulationCostOutputs.PerformLayout();
            this.grpSimulationUtilizationOutputs.ResumeLayout(false);
            this.grpSimulationUtilizationOutputs.PerformLayout();
            this.grpSimulationInputs.ResumeLayout(false);
            this.grpSimulationInputs.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSimulationHeader)).EndInit();
            this.tabDistribution.ResumeLayout(false);
            this.grpDistributionDefaults.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picDefaultLoadingDistribution)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDefaultTravelDistribution)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDefaultWeighingDistribution)).EndInit();
            this.grpDistributionNewValues.ResumeLayout(false);
            this.grpDistributionNewValues.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridTravelingDistribution)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridWeighingDistribution)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLoadingDistribution)).EndInit();
            this.mainTabControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tabGeneticAlgorithm;
        private System.Windows.Forms.GroupBox grpLegacyGaOutputs;
        private System.Windows.Forms.Label lblLegacyGaTotalCost;
        private System.Windows.Forms.Label lblLegacyGaTotalDays;
        private System.Windows.Forms.Label lblLegacyGaDelayCost;
        private System.Windows.Forms.Label lblLegacyGaDaysDelayed;
        private System.Windows.Forms.Label lblLegacyGaDaysDelayedCaption;
        private System.Windows.Forms.Label lblLegacyGaTotalCostCaption;
        private System.Windows.Forms.Label lblLegacyGaTotalDaysCaption;
        private System.Windows.Forms.Label lblLegacyGaDelayCostCaption;
        private System.Windows.Forms.Label lblLegacyGaUtilScalers;
        private System.Windows.Forms.Label lblLegacyGaUtilScalersCaption;
        private System.Windows.Forms.Label lblLegacyGaUtilLoaders;
        private System.Windows.Forms.Label lblLegacyGaUtilTrucks;
        private System.Windows.Forms.Label lblLegacyGaScalerCount;
        private System.Windows.Forms.Label lblLegacyGaUtilLoadersCaption;
        private System.Windows.Forms.Label lblLegacyGaUtilTrucksCaption;
        private System.Windows.Forms.Label lblLegacyGaScalerCountCaption;
        private System.Windows.Forms.Label lblLegacyGaLoaderCountCaption;
        private System.Windows.Forms.Label lblLegacyGaTruckCountCaption;
        private System.Windows.Forms.Label lblLegacyGaLoaderCount;
        private System.Windows.Forms.Label lblLegacyGaTruckCount;
        private System.Windows.Forms.Button btnRunGeneticAlgorithm;
        private System.Windows.Forms.Label lblGaProgressCaption;
        private System.Windows.Forms.ProgressBar progressGaRun;
        private System.Windows.Forms.GroupBox grpGaInputs;
        private System.Windows.Forms.Button btnClearGaFields;
        private System.Windows.Forms.Label lblGaMaterialVolume;
        private System.Windows.Forms.Label lblGaLoaderCostPerDay;
        private System.Windows.Forms.Label lblGaScalerCostPerDay;
        private System.Windows.Forms.TextBox txtGaGenerations;
        private System.Windows.Forms.Label lblGaTruckCostPerDay;
        private System.Windows.Forms.Label lblGaPopulation;
        private System.Windows.Forms.Label lblGaLoadPerTruck;
        private System.Windows.Forms.TextBox txtGaMutationRate;
        private System.Windows.Forms.Label lblGaProjectDuration;
        private System.Windows.Forms.Label lblGaGenerations;
        private System.Windows.Forms.Label lblGaDelayCostPerDay;
        private System.Windows.Forms.TextBox txtGaPopulation;
        private System.Windows.Forms.Label lblGaMutationRate;
        private System.Windows.Forms.TextBox txtGaMaterialVolume;
        private System.Windows.Forms.TextBox txtGaLoadPerTruck;
        private System.Windows.Forms.TextBox txtGaMaxScalers;
        private System.Windows.Forms.TextBox txtGaTruckCostPerDay;
        private System.Windows.Forms.Label lblGaMaxScalers;
        private System.Windows.Forms.TextBox txtGaLoaderCostPerDay;
        private System.Windows.Forms.TextBox txtGaMaxLoaders;
        private System.Windows.Forms.TextBox txtGaScalerCostPerDay;
        private System.Windows.Forms.Label lblGaMaxLoaders;
        private System.Windows.Forms.TextBox txtGaProjectDuration;
        private System.Windows.Forms.TextBox txtGaMaxTrucks;
        private System.Windows.Forms.TextBox txtGaDelayCostPerDay;
        private System.Windows.Forms.Label lblGaMaxTrucks;
        private System.Windows.Forms.Button btnRunGaDemo;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartGaFitness;
        private System.Windows.Forms.PictureBox picGaHeader;
        private System.Windows.Forms.TabPage tabSimulation;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartSimulationCosts;
        private System.Windows.Forms.Button btnClearSimulationFields;
        private System.Windows.Forms.GroupBox grpSimulationCostOutputs;
        private System.Windows.Forms.Label lblSimDaysDelayedValue;
        private System.Windows.Forms.Label lblSimDaysDelayedCaption;
        private System.Windows.Forms.Label lblSimProjectDurationCaption;
        private System.Windows.Forms.Label lblSimLoaderCostCaption;
        private System.Windows.Forms.Label lblSimScalerCostCaption;
        private System.Windows.Forms.Label lblSimTotalCostValue;
        private System.Windows.Forms.Label lblSimDelayCostCaption;
        private System.Windows.Forms.Label lblSimTotalCostCaption;
        private System.Windows.Forms.Label lblSimTruckCostCaption;
        private System.Windows.Forms.Label lblSimLoaderCostValue;
        private System.Windows.Forms.Label lblSimTruckCostValue;
        private System.Windows.Forms.Label lblSimDelayCostValue;
        private System.Windows.Forms.Label lblSimScalerCostValue;
        private System.Windows.Forms.Label lblSimProjectDurationValue;
        private System.Windows.Forms.GroupBox grpSimulationUtilizationOutputs;
        private System.Windows.Forms.Label lblSimUtilTrucksCaption;
        private System.Windows.Forms.Label lblSimUtilScalersCaption;
        private System.Windows.Forms.Label lblSimUtilLoadersCaption;
        private System.Windows.Forms.Label lblSimUtilTrucksValue;
        private System.Windows.Forms.Label lblSimUtilLoadersValue;
        private System.Windows.Forms.Label lblSimUtilScalersValue;
        private System.Windows.Forms.GroupBox grpSimulationInputs;
        private System.Windows.Forms.TextBox txtSimScalerCount;
        private System.Windows.Forms.Label lblSimScalerCount;
        private System.Windows.Forms.TextBox txtSimLoaderCount;
        private System.Windows.Forms.Label lblSimLoaderCount;
        private System.Windows.Forms.TextBox txtSimTruckCount;
        private System.Windows.Forms.Label lblSimTruckCount;
        private System.Windows.Forms.TextBox txtSimLoaderCostPerDay;
        private System.Windows.Forms.TextBox txtSimDelayCostPerDay;
        private System.Windows.Forms.TextBox txtSimProjectDuration;
        private System.Windows.Forms.TextBox txtSimScalerCostPerDay;
        private System.Windows.Forms.TextBox txtSimTruckCostPerDay;
        private System.Windows.Forms.TextBox txtSimLoadPerTruck;
        private System.Windows.Forms.TextBox txtSimMaterialVolume;
        private System.Windows.Forms.Label lblSimMaterialVolume;
        private System.Windows.Forms.Label lblSimDelayCostPerDay;
        private System.Windows.Forms.Label lblSimProjectDuration;
        private System.Windows.Forms.Label lblSimLoadPerTruck;
        private System.Windows.Forms.Label lblSimTruckCostPerDay;
        private System.Windows.Forms.Label lblSimScalerCostPerDay;
        private System.Windows.Forms.Label lblSimLoaderCostPerDay;
        private System.Windows.Forms.Button btnRunSimulation;
        private System.Windows.Forms.PictureBox picSimulationHeader;
        private System.Windows.Forms.TabPage tabDistribution;
        private System.Windows.Forms.GroupBox grpDistributionDefaults;
        private System.Windows.Forms.PictureBox picDefaultLoadingDistribution;
        private System.Windows.Forms.PictureBox picDefaultTravelDistribution;
        private System.Windows.Forms.PictureBox picDefaultWeighingDistribution;
        private System.Windows.Forms.GroupBox grpDistributionNewValues;
        private System.Windows.Forms.Label lblTravelingDistributionHeader;
        private System.Windows.Forms.Label lblWeighingDistributionHeader;
        private System.Windows.Forms.Label lblLoadingDistributionHeader;
        private System.Windows.Forms.Button btnClearDistributionGrids;
        private System.Windows.Forms.Button btnAddDistributionsToSimulation;
        private System.Windows.Forms.DataGridView gridTravelingDistribution;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTravelingTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTravelingProbability;
        private System.Windows.Forms.DataGridView gridWeighingDistribution;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWeighingTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWeighingProbability;
        private System.Windows.Forms.DataGridView gridLoadingDistribution;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLoadingTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLoadingProbability;
        private System.Windows.Forms.TabControl mainTabControl;
    }
}