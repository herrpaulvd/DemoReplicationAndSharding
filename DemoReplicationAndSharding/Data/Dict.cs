using System.ComponentModel.DataAnnotations.Schema;

namespace DemoReplicationAndSharding.Data
{
    [Table("dict")]
    public class Dict
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
