using System;
using System.Collections.Generic;

namespace VueApp1.Server.Models;

public partial class T_BZJ_JCXX
{
    public string F_ID { get; set; } = null!;

    public string F_BLH { get; set; } = null!;

    public string? F_SJDW { get; set; }

    public string? F_SJDW_DM { get; set; }

    public string? F_BRBH { get; set; }

    public string? F_SQXH { get; set; }

    public string F_XB { get; set; } = null!;

    public string? F_NL { get; set; }

    public double? F_AGE { get; set; }

    public string? F_HY { get; set; }

    public string? F_LCZD { get; set; }

    public string? F_LCZL { get; set; }

    public string? F_RYSJ { get; set; }

    public string? F_JXSJ { get; set; }

    public string? F_BLZD { get; set; }

    public string? F_TSJC { get; set; }

    public string? F_ZDGJC { get; set; }

    public string? F_SJCL { get; set; }

    public string? F_ICD10_BM1 { get; set; }

    public string? F_ICD10_MC1 { get; set; }

    public string? F_ICD10_BM2 { get; set; }

    public string? F_ICD10_MC2 { get; set; }

    public string? F_BW { get; set; }

    public string? F_BM { get; set; }

    public string F_ZT { get; set; } = null!;

    public string? F_CREATE_BY { get; set; }

    public DateTime? F_CREATE_TIME { get; set; }

    public string? F_UPDATE_BY { get; set; }

    public DateTime? F_UPDATE_TIME { get; set; }

    public string? F_CREATE_BY_NAME { get; set; }

    public string? F_UPDATE_BY_NAME { get; set; }

    public string? F_FHR { get; set; }

    public DateTime? F_FHSJ { get; set; }

    public string? F_YW_BM { get; set; }

    public string? F_WHO_BH { get; set; }

    public long? F_PXH { get; set; }

    public string? F_GUID { get; set; }

    public string? F_QPJX_ID { get; set; }

    public string? F_BZJX_ID { get; set; }

    public string? F_LWXY_SFTB { get; set; }

    /// <summary>
    /// 是否用AI分析 1-是 其他-否
    /// </summary>
    public int? F_AI_MARK { get; set; }
}
