using System;
using System.Collections.Generic;

namespace VueApp1.Server.Models;

public partial class TNM_AI_RESULT
{
    public string ID { get; set; } = null!;

    public string JCXX_ID { get; set; } = null!;

    /// <summary>
    /// tnm分期标准的名称，比如胃癌，肺癌
    /// </summary>
    public string? TNM_RULE_NAME { get; set; }

    /// <summary>
    /// AI返回的全文
    /// </summary>
    public string? FULL_AI_RESULT { get; set; }

    /// <summary>
    /// T分期结果
    /// </summary>
    public string? T_LEVEL { get; set; }

    /// <summary>
    /// T分期判断依据
    /// </summary>
    public string? T_DESC { get; set; }

    /// <summary>
    /// N分期结果
    /// </summary>
    public string? N_LEVEL { get; set; }

    /// <summary>
    /// N分期判断依据
    /// </summary>
    public string? N_DESC { get; set; }

    /// <summary>
    /// M分期结果
    /// </summary>
    public string? M_LEVEL { get; set; }

    /// <summary>
    /// M分期判断依据
    /// </summary>
    public string? M_DESC { get; set; }

    /// <summary>
    /// 错误信息
    /// </summary>
    public string? ERROR_MESSAGE { get; set; }

    /// <summary>
    /// AI对分期判断准确性的信心，评分从1到5，分数越高表示信心越大
    /// </summary>
    public int? CONFIDENCE_LEVEL { get; set; }

    /// <summary>
    /// 如果无法准确判断，请在此字段中提出你的建议
    /// </summary>
    public string? SUGGESTION { get; set; }

    public DateTime? Create_time { get; set; }
}
