using System;
using System.Collections.Generic;

namespace VueApp1.Server.Models;

public partial class TNM_RULE_DICT
{
    public string ID { get; set; } = null!;

    /// <summary>
    /// Tnm分期标准名称
    /// </summary>
    public string? TNM_RULE_NAME { get; set; }

    /// <summary>
    /// Tnm分期标准内容
    /// </summary>
    public string? TNM_RULE_CONTENT { get; set; }
}
