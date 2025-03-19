using Assets.UiTest.Context;

namespace Assets.UiTest.TestSteps
{
    public class Steps
    {
        public Steps(IUiTestContext context)
        {
            UiTestStepBase.SetContext(context);
        }

        public IUiTestStepBase WaitStartLoadingStep()
        {
            return new WaitStartLoadingStep();
        }

        public IUiTestStepBase ExampleStep()
        {
            return new ExampleStep();
        }
        public IUiTestStepBase InventoryStep()
        {
            return new InventoryStep();
        }
    }
}