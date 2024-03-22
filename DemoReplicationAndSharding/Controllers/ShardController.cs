using DemoReplicationAndSharding.Data;
using Microsoft.AspNetCore.Mvc;

namespace DemoReplicationAndSharding.Controllers
{
    [Route("shard")]
    public class ShardController : Controller
    {
        public class Model
        {
            public string GetIndex { get; set; } = "";
            public string GetElement { get; set; } = "";
            public string AddIndex { get; set; } = "";
            public string AddElement { get; set; } = "";
        }

        private IActionResult ConstructPage(string gi, string ge, string ai, string ae)
        {
            return View("Shard", new Model { AddElement = ae, GetElement = ge, GetIndex = gi, AddIndex = ai });
        }

        private ShardContext[] contexts =
        {
            new(0), new(1), new(2)
        };

        private ShardContext GetContext(string key) => contexts[ShardContext.GetShardIndex(key)];

        [Route("index")]
        public IActionResult Index()
        {
            return ConstructPage("", "", "", "");
        }

        [HttpPost]
        [Route("get")]
        public IActionResult PostGet()
        {
            var form = Request.Form;
            string gi = form["gi"];
            string ge = GetContext(gi).Dict.First(e => e.Key == gi).Value;
            return ConstructPage(gi, ge, "", "");
        }

        [HttpPost]
        [Route("add")]
        public IActionResult PostAdd()
        {
            var form = Request.Form;
            string ae = form["ae"];
            string ai = form["ai"];
            Dict dict = new() { Key = ai, Value = ae };
            var ctx = GetContext(ai);
            ctx.Add(dict);
            ctx.SaveChanges();
            return ConstructPage("", "", ai, ae);
        }
    }
}
