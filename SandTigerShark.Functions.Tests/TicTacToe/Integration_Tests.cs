using FakeItEasy;
using FluentAssertions;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SandTigerShark.Functions.Tests.TicTacToe
{
    //http://www.ben-morris.com/writing-unit-tests-for-azure-functions-using-c/
    [TestClass]
    public class TicTacToe_function
    {
        [TestMethod]
        public async Task returns_a_200_OK_for_a_valid_play()
        {
            var command = PlayCommand.New();
            var request = HttpRequest.Create(command);

            var result = await Functions.TicTacToe.TicTacToe.Run(request, A.Fake<TraceWriter>());
            var response = (HttpResponseMessage)result;

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}