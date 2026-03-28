using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HusRoizBackendTest_MidLevel.Models
{
    public class MyofficeAcpdDto
    {
        [Required]
        [StringLength(60)]
        [DefaultValue("管理員小明")]
        public string? AcpdCname { get; set; }

        [StringLength(40)]
        [DefaultValue("AdminMing")]
        public string? AcpdEname { get; set; }

        [StringLength(40)]
        [DefaultValue("Ming")]
        public string? AcpdSname { get; set; }

        [EmailAddress]
        [StringLength(60)]
        [DefaultValue("ming@example.com")]
        public string? AcpdEmail { get; set; }

        [DefaultValue(0)]
        public byte? AcpdStatus { get; set; }

        [DefaultValue(false)]
        public bool? AcpdStop { get; set; }

        [StringLength(600)]
        [DefaultValue("")]
        public string? AcpdStopMemo { get; set; }

        [StringLength(30)]
        [DefaultValue("admin_ming")]
        public string? AcpdLoginID { get; set; }

        [StringLength(60)]
        [DefaultValue("password123")]
        public string? AcpdLoginPW { get; set; }

        [StringLength(120)]
        [DefaultValue("測試帳號建立")]
        public string? AcpdMemo { get; set; }

        [StringLength(20)]
        [DefaultValue("SYSTEM")]
        public string? AppdNowid { get; set; }

        [StringLength(20)]
        [DefaultValue("SYSTEM")]
        public string? AcpdUpdid { get; set; }
    }
}
