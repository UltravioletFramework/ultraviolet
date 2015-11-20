using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace UvTestRunner.Models
{
    public class TestRun
    {
        [Column("TestRunID")]
        public Int64 ID { get; set; }
        public TestRunStatus Status { get; set; }
    }
}
