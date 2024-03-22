using DemoReplicationAndSharding.Data;
using Microsoft.AspNetCore.Mvc;

namespace DemoReplicationAndSharding.Controllers
{
    [Controller]
    [Route("repl")]
    public class ReplController : Controller
    {
        public class Model
        {
            public int GetIndex { get; set; } = 1;
            public string GetElement { get; set; } = "";
            public string AddElement { get; set; } = "";
        }

        private IActionResult ConstructPage(int gi, string ge, string ae)
        {
            return View("Repl", new Model { AddElement = ae, GetElement = ge, GetIndex = gi });
        }

        private ReplContext master = new(ReplContext.Master);
        private ReplContext slave = new(ReplContext.Slave);
        private ReplContext slave2 = new(ReplContext.Slave2);
        private Random random = new();

        private ReplContext GetContext() => random.Next(3) switch
        {
            ReplContext.Master => master,
            ReplContext.Slave => slave,
            _ => slave2,
        };

        [Route("index")]
        public IActionResult Index()
        {
            return ConstructPage(1, "", "");
        }

        [HttpPost]
        [Route("get")]
        public IActionResult PostGet()
        {
            var form = Request.Form;
            int gi = int.Parse(form["gi"]);
            string ge = GetContext().Arr.First(e => e.Index == gi).Element;
            return ConstructPage(gi, ge, "");
        }

        [HttpPost]
        [Route("add")]
        public IActionResult PostAdd()
        {
            var form = Request.Form;
            string ae = form["ae"];
            Arr arr = new() { Element = ae };
            master.Arr.Add(arr);
            master.SaveChanges();
            return ConstructPage(1, "", ae);
        }
    }
}
