using System.Collections.Generic;

namespace Assets.UiTest.Runner
{
    public class UiTestCases : IUiTestCases
    {
        private Dictionary<int, IUiTestCase> _tests = new Dictionary<int, IUiTestCase>();

        public UiTestCases()
        {
            _tests.Add(0, new TestCase0());
            _tests.Add(1, new TestCase1());
            _tests.Add(2, new TestCase2());
            _tests.Add(3, new TestCase3());
            _tests.Add(4, new TestCase4());
        }

        public IUiTestCase GetTestCase(int test)
        {
            return _tests[test];
        }
    }
}