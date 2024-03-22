using System.ComponentModel.DataAnnotations.Schema;

namespace DemoReplicationAndSharding.Data
{
    [Table("arr")]
    public class Arr
    {
        public int Index { get; set; }
        public string Element { get; set; }
    }
}
