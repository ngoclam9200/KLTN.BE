using System.ComponentModel.DataAnnotations.Schema;

namespace DoubleLStore.WebApp.Entities
{
    public class SalaryStaff
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public int NumberOfWorking { get; set; }
        public string ListDayWorking { get; set; }
        public string Month { get; set; }
        public string SalaryOfThisMonth { get; set; }
        public string Salary { get; set; }
        public bool isWorking { get; set; }
        public string StaffId { get; set; }
        public Staffs Staff { get; set; }

    }
}
