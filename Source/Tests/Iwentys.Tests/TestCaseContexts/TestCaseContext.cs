namespace Iwentys.Tests.TestCaseContexts
{
    public class TestCaseContext
    {
        public readonly AccountManagementTestCaseContext AccountManagementTestCaseContext;
        public readonly GithubTestCaseContext GithubTestCaseContext;

        public readonly StudyTestCaseContext StudyTestCaseContext;

        public TestCaseContext()
        {
            GithubTestCaseContext = new GithubTestCaseContext();
            AccountManagementTestCaseContext = new AccountManagementTestCaseContext(this);
            StudyTestCaseContext = new StudyTestCaseContext();
        }

        public static TestCaseContext Case()
        {
            return new TestCaseContext();
        }
    }
}