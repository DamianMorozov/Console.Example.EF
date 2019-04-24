using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Console.Example.EF.DataModel.Tables
{
    [Table("IRIS")]
    public class Iris
    {
        [Key]
        public int ID { get; set; }
        public float SEPAL_LENGTH { get; set; }
        public float SEPAL_WIDTH { get; set; }
        public float PETAL_LENGTH { get; set; }
        public float PETAL_WIDTH { get; set; }
        public string LABEL { get; set; }

        public override string ToString()
        {
            return 
                $"SepalLength: {SEPAL_LENGTH}" +
                $"; SepalWidth: {SEPAL_WIDTH}" +
                $"; PetalLength: {PETAL_LENGTH}" +
                $"; PetalWidth: {PETAL_WIDTH}" + 
                $"; Label: {LABEL}.";
        }
    }
}
