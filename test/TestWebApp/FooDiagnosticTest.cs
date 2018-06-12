using System.Threading;
using System.Threading.Tasks;
using Webenable.Diagnostics;

namespace TestWebApp
{
    public class FooDiagnosticTest : DiagnosticTest
    {
        public override Task<DiagnosticTestResult> Execute(CancellationToken cancellationToken)
        {
            return Task.FromResult(DiagnosticTestResult.Success());
        }
    }
}
