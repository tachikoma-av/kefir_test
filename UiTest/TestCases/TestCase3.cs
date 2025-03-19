using System.Collections.Generic;
using Assets.UiTest.TestSteps;

namespace Assets.UiTest.Runner;

public class TestCase3 : UiStepsTestCase
{
    /*
    main purpose of this test case is to check the inventory basic functionality
    */
    protected override IEnumerator<IUiTestStepBase> Condition()
    {
        yield return Steps.WaitStartLoadingStep();
        yield return Steps.AxeStep();
    }
}