namespace SpecBind.Steps
{
    using System.Collections.Generic;
    using System.Linq;

    using SpecBind.ActionPipeline;
    using SpecBind.Actions;

    using TechTalk.SpecFlow;
    using Context;
    using Extensions;
    using Control;

    /// <summary>
    /// Step bindings related to entering or validating data.
    /// </summary>
    [Binding]
    public class DataSteps : StepBase
    {
        private readonly IActionPipelineService actionPipelineService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSteps"/> class.
        /// </summary>
        /// <param name="scenarioContext">The scenario context.</param>
        /// <param name="actionPipelineService">The action pipeline service.</param>
        public DataSteps(IContext scenarioContext, IActionPipelineService actionPipelineService)
            : base(scenarioContext)
        {
            this.actionPipelineService = actionPipelineService;
        }

        /// <summary>
        /// Given I entered data.
        /// </summary>
        /// <param name="data">The field data.</param>
        [Given("I entered data")]
        public void EnterDataInFieldsStep(Table data)
        {
            string fieldHeader = null;
            string valueHeader = null;

            if (data != null)
            {
                fieldHeader = data.Header.FirstOrDefault(h => h.NormalizedEquals("Field"));
                valueHeader = data.Header.FirstOrDefault(h => h.NormalizedEquals("Value"));
            }

            if (fieldHeader == null || valueHeader == null)
            {
                throw new ControlExecuteException("A table must be specified for this step with the columns 'Field' and 'Value'");
            }

            var application = this.GetApplicationFromContext();
            var window = this.GetWindowFromContext();

            var results = new List<ActionResult>(data.RowCount);
            results.AddRange(from tableRow in data.Rows
                             let fieldName = tableRow[fieldHeader]
                             let fieldValue = tableRow[valueHeader]
                             select new EnterDataAction.EnterDataContext(fieldName.ToLookupKey(), fieldValue) into context
                             select this.actionPipelineService.PerformAction<EnterDataAction>(application, window, context));

            if (results.Any(r => !r.Success))
            {
                var errors = string.Join("; ", results.Where(r => r.Exception != null).Select(r => r.Exception.Message));
                throw new ControlExecuteException("Errors occurred while entering data. Details: {0}", errors);
            }
        }
    }
}