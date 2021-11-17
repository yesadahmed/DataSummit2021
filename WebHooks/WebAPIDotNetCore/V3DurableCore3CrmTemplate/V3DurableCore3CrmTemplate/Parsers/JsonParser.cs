using V3DurableCore3CrmTemplate.Model;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace V3DurableCore3CrmTemplate.Parsers
{
	public class JsonParser
	{

		#region Data Member  
		ILogger log;
		#endregion


		#region Constructor
		public JsonParser(ILogger Log)
		{
			log = Log;
		}

		#endregion

		#region Public Functions  
		/// <summary>
		/// you can extend this as much as you want
		/// </summary>
		/// <param name="webhookJson"></param>
		/// <returns></returns>
		public AccountModel GetInfoOnCreateOrUpdateOrDelete(string webhookJson)
		{
			AccountModel caseModel = null;
			try
			{
				dynamic data = JsonConvert.DeserializeObject(ParseJson(webhookJson));//Trim
				if (data != null)
				{
					caseModel = new AccountModel();
					caseModel.PrimaryEntityId = data != null ? data.PrimaryEntityId : "";
					caseModel.PrimaryEntityName = data != null ? data.PrimaryEntityName : "";
					caseModel.MessageName = data != null ? data.MessageName : "";

					if (!caseModel.PrimaryEntityName.Equals("col_workrelationship"))
					{
						log.LogError($"Entity is not incident.");//just avoid and return task
						return null;
					}

					if (!string.IsNullOrEmpty(caseModel.MessageName))
					{

						if (caseModel.MessageName.ToLower().Equals("create"))
						{
							caseModel = GetAttributesValuesFromPrePostImages(caseModel, data);//  image attributes  only
							caseModel.IsCreateMessage = true;

						}
						else if (caseModel.MessageName.ToLower().Equals("update"))
						{
							caseModel = GetAttributesValuesFromPrePostImages(caseModel, data); //image attributes  only

							if (!caseModel.precol_accountid2.Equals(caseModel.col_accountid2, StringComparison.OrdinalIgnoreCase))
								caseModel.IsUpdateMessage = true;

						}
						else if (caseModel.MessageName.ToLower().Equals("delete"))//image attributes  only
						{
							caseModel = GetAttributesValuesFromPrePostImages(caseModel, data);
							//nothign need here for now
							caseModel.IsDeleteMessage = true;
						}

						if ((data["InputParameters"] != null) && data["InputParameters"][0] != null) //Define your own attributes in model
						{
							caseModel = GetInputParameterValues(caseModel, data);
						}

						if ((data["OutputParameters"] != null) && data["OutputParameters"][0] != null) //Define your own attributes in model
						{
							caseModel = GetOutputParametersValues(caseModel, data);
						}
					}
				}
			}
			catch (Exception ex)
			{

				log.LogError($"Error:GetRealsjonInfoOnCreateOrUpdate parsing:  {ex.Message}");
			}

			return caseModel;

		}
		#endregion

		#region Private Functions

		private AccountModel GetAttributesValuesFromPrePostImages(AccountModel caseModel, dynamic data)
		{
			AccountModel casedata = caseModel;//copy everything

			if (data.PostEntityImages != null && (data.PostEntityImages.Count > 0) && data.PostEntityImages[0] != null)
			{

				if (data.PostEntityImages[0].key == "postimage".ToLower())//important what we expect
				{
					if (data.PostEntityImages[0].value != null)
					{
						if (data.PostEntityImages[0].value.Attributes != null && data.PostEntityImages[0].value.Attributes[0] != null)
						{
							foreach (var att in data.PostEntityImages[0].value.Attributes)
							{
								if (att.key == "col_accountid2")//Lookup
								{
									if (att.value.Id != null)
										caseModel.col_accountid2 = Convert.ToString(att.value.Id);//	

									if (att.value.Name != null)
										caseModel.col_accountid2Name = Convert.ToString(att.value.Name);//	in case you need name
								}


								if (att.key == "statuscode")// (optionset)
								{
									if (att.value != null && att.value.Value != null)
										if (att.value.Value.Value != null)
											caseModel.statuscode = Convert.ToInt32(att.value.Value.Value);//									
								}

								if (att.key == "title")// string (straight)
								{
									if (att.value != null)
										caseModel.title = Convert.ToString(att.value);//									
								}

							}
						}
					}
				}


			}//PostEntityImages
			else
			{
				caseModel.precol_accountid2 = "";
				caseModel.precol_accountid2Name = "";


			}


			if (data.PreEntityImages != null && (data.PreEntityImages.Count > 0) && data.PreEntityImages[0] != null)
			{
				if (data.PreEntityImages[0].key != null)
				{

					if (data.PreEntityImages[0].value != null)
					{
						if (data.PreEntityImages[0].value.Attributes != null && data.PreEntityImages[0].value.Attributes[0] != null)
						{
							foreach (var att in data.PreEntityImages[0].value.Attributes)
							{
								if (att.key == "col_accountid2")//we need this only for now (lookup)
								{
									if (att.value.Id != null)
										caseModel.precol_accountid2 = Convert.ToString(att.value.Id);//	

									if (att.value.Name != null)
										caseModel.precol_accountid2Name = Convert.ToString(att.value.Name);//	in case you need name							
								}


								if (att.key == "statuscode")// we need this only for now (optionset)
								{
									if (att.value != null && att.value.Value != null)
										if (att.value.Value.Value != null)
											caseModel.prestatuscode = Convert.ToInt32(att.value.Value.Value);//									
								}

								if (att.key == "title")// string (straight)
								{
									if (att.value != null)
										caseModel.pretitle = Convert.ToString(att.value);//									
								}

							}
						}
					}


				}
			}//PreEntityImages
			else
			{
				caseModel.precol_accountid2 = "";
				caseModel.precol_accountid2Name = "";
			}

			// make sure anycase the pre field must be empty not null (avoid exception)
			if (string.IsNullOrWhiteSpace(caseModel.precol_accountid2Name)) //double check again			
				caseModel.precol_accountid2Name = "";


			if (string.IsNullOrWhiteSpace(caseModel.precol_accountid2)) //double check againin case post delete	
				caseModel.precol_accountid2 = "";




			return caseModel;
		}


		private AccountModel GetInputParameterValues(AccountModel caseModel, dynamic data)
		{
			AccountModel casedata = caseModel;//copy everything

			if (data.InputParameters != null && (data.InputParameters.Count > 0) && data.InputParameters[0] != null)
			{
				if (data.InputParameters[0].key != null)
				{
					if (data.InputParameters[0].key == "Target")//important what we expect
					{
						if (data.InputParameters[0].value != null)
						{
							if (data.InputParameters[0].value.Attributes != null && data.InputParameters[0].value.Attributes[0] != null)
							{
								foreach (var att in data.InputParameters[0].value.Attributes)
								{
									if (att.key == "col_accountid2")//Lookup
									{
										if (att.value.Id != null)
											caseModel.col_accountid2 = Convert.ToString(att.value.Id);//	

										if (att.value.Name != null)
											caseModel.col_accountid2Name = Convert.ToString(att.value.Name);//	in case you need name
									}


									if (att.key == "statuscode")// (optionset)
									{
										if (att.value != null && att.value.Value != null)
											if (att.value.Value.Value != null)
												caseModel.statuscode = Convert.ToInt32(att.value.Value.Value);//									
									}

									if (att.key == "title")// string (straight)
									{
										if (att.value != null)
											caseModel.title = Convert.ToString(att.value);//									
									}

								}
							}
						}
					}

				}
			}//InputParameters

			return caseModel;
		}


		private AccountModel GetOutputParametersValues(AccountModel caseModel, dynamic data)
		{
			AccountModel casedata = caseModel;//copy everything

			if (data.OutputParameters != null && (data.OutputParameters.Count > 0) && data.OutputParameters[0] != null)
			{
				if (data.OutputParameters[0].key != null)
				{
					if (data.OutputParameters[0].key == "Target")//important what we expect
					{
						if (data.OutputParameters[0].value != null)
						{
							if (data.OutputParameters[0].value.Attributes != null && data.OutputParameters[0].value.Attributes[0] != null)
							{
								foreach (var att in data.OutputParameters[0].value.Attributes)
								{
									if (att.key == "col_accountid2")//Lookup
									{
										if (att.value.Id != null)
											caseModel.col_accountid2 = Convert.ToString(att.value.Id);//	

										if (att.value.Name != null)
											caseModel.col_accountid2Name = Convert.ToString(att.value.Name);//	in case you need name
									}


									if (att.key == "statuscode")// (optionset)
									{
										if (att.value != null && att.value.Value != null)
											if (att.value.Value.Value != null)
												caseModel.statuscode = Convert.ToInt32(att.value.Value.Value);//									
									}

									if (att.key == "title")// string (straight)
									{
										if (att.value != null)
											caseModel.title = Convert.ToString(att.value);//									
									}

								}
							}
						}
					}

				}
			}//OutputParameters


			return caseModel;
		}
		string ParseJson(string unformattedJson)
		{
			string formattedJson = string.Empty;
			try
			{
				formattedJson = unformattedJson.Trim('"');
				formattedJson = System.Text.RegularExpressions.Regex.Unescape(formattedJson);
			}
			catch (Exception ex)
			{


			}
			return formattedJson;
		}
		#endregion

	}
}
