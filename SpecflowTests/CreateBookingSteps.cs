using Xunit;
using TechTalk.SpecFlow;

Namespace SpecflowTests {

    [Binding]
    public class CreateBookingSteps
{

    [Given(@"I have entered a start date(.*)")]
    public void GivenIHaveEnteredA(string strDate) {

        startDate = DateTime.Parse(strDate);
    }

        <TechTalk.SpecFlow.When("I press button Create booking")> _
        Public Sub WhenIPressButtonCreateBooking()
            ScenarioContext.Current.Pending()
        End Sub

        <TechTalk.SpecFlow.Then("The result should be (.*)")> _
        Public Sub ThenTheResultShouldBe(ByVal p0 As String)
            ScenarioContext.Current.Pending()
        End Sub

    }
}