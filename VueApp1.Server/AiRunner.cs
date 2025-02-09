using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VueApp1.Server.Helpers;
using VueApp1.Server.Models;

namespace AiTNMHelper
{
    public class AiRunner
    {
        // Existing code...



        public static CancerTypeResponse QuestCancerType(string jcxxInfo)
        {
            // 1. 生成文本示例
            var generateReq = new OllamaApiHelper.GenerateRequest
            {
                Model = "deepseek-r1:32b",
                Prompt = $@"你的任务是根据提供的病例信息，判断该病例适用哪一种pTNM分期标准，并严格按照以下格式返回结果：

    如果能匹配到标准，返回格式为：
    <分期标准>分期标准名称</分期标准>
    其中，分期标准名称必须为以下选项之一：
    口腔癌 | 鼻咽癌 | 口咽癌 | 喉癌 | 鼻窦癌 | 唾液腺癌 | 甲状腺癌 | 乳腺癌 | 黑色素瘤 | 默克尔细胞癌 | 软组织肉瘤 | 骨肉瘤 | 非小细胞肺癌 | 小细胞肺癌 | 食管癌及食管胃交界癌 | 胃癌 | 结直肠癌 | 肛门癌 | 肝细胞癌 | 肝内胆管癌 | 胆囊癌 | 胆管癌 | 胰腺癌 | 胰腺神经内分泌肿瘤 | 肾细胞癌 | 膀胱癌 | 前列腺癌 | 睾丸癌 | 阴茎癌 | 宫颈癌 | 子宫内膜癌 | 卵巢癌 | 外阴癌 | 阴道癌 | 恶性胸膜间皮瘤 | 眼黑色素瘤

    如果无法匹配，返回格式为：
    <错误原因>无法匹配到适用的分期标准，原因：具体原因描述</错误原因>

    请只返回以上格式的结果，不要输出额外信息。

    以下是病例信息：{jcxxInfo}

    ",
                Stream = false
            };
            var generateResp = OllamaApiHelper.GenerateTextAsync(generateReq);

            // 2. 解析AI回答
            var result = new CancerTypeResponse();
            var responseText = generateResp.Result.Response;

          //  Logger.Info($"[GenerateText] AI回答: {responseText}\n");

            // 提取分期标准
            var extractedContent = CommonHelper.ExtractContent(responseText, "分期标准");
            if (extractedContent != null)
            {
                result.Tnm_Rule_Name = extractedContent;
                result.FullResult = responseText;
            }
            else
            {
                // 提取错误原因
                extractedContent = CommonHelper.ExtractContent(responseText, "错误原因");
                if (extractedContent != null)
                {
                    result.Tnm_Rule_Name = "无法匹配";
                    result.FullResult = extractedContent;
                }
                else
                {
                    result.Tnm_Rule_Name = "未知错误";
                    result.FullResult = "无法解析AI返回的结果。";
                }
            }

            return result;
        }

        public static TnmResponse QuestTnm(string tnmRuleName,string jcxxInfo)
        {
            var tnmResponse = new TnmResponse();

            //在数据库中查找tnmRule数据,如果找不到,直接返回异常信息
            var rule = MsSqlHelper.ExecuteScalar($@"select TNM_RULE_CONTENT from TNM_RULE_DICT where TNM_RULE_NAME='{tnmRuleName}'");
            if (string.IsNullOrEmpty(rule))
            {
                tnmResponse.ERROR_MESSAGE = $"未找到名为[{tnmRuleName}]的TNM规则";
                return tnmResponse;
            }

            // 1. 生成文本示例
            var generateReq = new OllamaApiHelper.GenerateRequest
            {
                Model = "deepseek-r1:32b",
                Prompt = $@"你的任务是根据下面提供的TNM分期规则和病例信息，对该病例进行TNM分期判断，不需要判断M，因为是分析病理科的诊断。
请注意以下要求：

对T、N各项进行判断时，如果无法确定某一项的具体分期，请不要给出明确的分期名称，而应在对应的描述标签中说明原因。
如果你对某一个分诶的判断不确定,可以在分期后面加一个?,比如T1?,N2?

请严格按照以下格式返回结果，不要包含其他任何信息或解释：


<T_LEVEL>T分期名称，如T1、T2、T2a…</T_LEVEL>
<T_DESC>T分期的依据</T_DESC>
<N_LEVEL>N分期名称，如N1、N2、N2a…</N_LEVEL>
<N_DESC>N分期的依据</N_DESC>
<CONFIDENCE_LEVEL>你对分期判断准确性的信心，评分从1到5，分数越高表示信心越大</CONFIDENCE_LEVEL>
<SUGGESTION>如果无法准确判断，请在此字段中提出你的建议</SUGGESTION>

请只返回上述格式的内容，不要输出额外信息。

以下是TNM规则：{rule}

以下是病例信息：{jcxxInfo}

    ",
                Stream = false
            };
            var generateResp = OllamaApiHelper.GenerateTextAsync(generateReq);

            // 2. 解析AI回答
            var result = new TnmResponse();
            var responseText = generateResp.Result.Response;

            //Logger.Info($"[GenerateText] AI回答: {responseText}\n");

            result.TNM_RULE_NAME = tnmRuleName;
            result.FULL_AI_RESULT = responseText;
            result.T_LEVEL = CommonHelper.ExtractContent(responseText, "T_LEVEL");
            result.T_DESC = CommonHelper.ExtractContent(responseText, "T_DESC");
            result.N_LEVEL = CommonHelper.ExtractContent(responseText, "N_LEVEL");
            result.N_DESC = CommonHelper.ExtractContent(responseText, "N_DESC");
            result.CONFIDENCE_LEVEL = CommonHelper.ExtractContentAsInt(responseText, "CONFIDENCE_LEVEL");
            result.SUGGESTION = CommonHelper.ExtractContent(responseText, "SUGGESTION");

            return result;
        }


        public class CancerTypeResponse
        {
            public string Tnm_Rule_Name { get; set; }
            public string FullResult { get; set; }
        }

        public class TnmResponse: TNM_AI_RESULT
        {

        }
    }
}
     