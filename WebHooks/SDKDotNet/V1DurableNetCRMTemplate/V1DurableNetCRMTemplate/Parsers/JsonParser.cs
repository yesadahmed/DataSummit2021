using V1DurableNetCRMTemplate.Model;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace V1DurableNetCRMTemplate.Parsers
{
	public class JsonParser
	{

		#region Data Member  
		TraceWriter log;
		#endregion


		#region Constructor
		public JsonParser(TraceWriter Log)
		{
			log = Log;
		}

		#endregion

		#region Public Functions  
		/// <summary>
		/// you can exten this as much as you want
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

					if (!caseModel.PrimaryEntityName.Equals("account"))
					{
						log.Error($"Entity is not account.");//just avoid and return task
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

							if (caseModel.prenew_products != caseModel.new_products)
								caseModel.IsUpdateMessage = true;

							if (!caseModel.prenew_proimageurl.Equals(caseModel.new_proimageurl, StringComparison.OrdinalIgnoreCase))
								caseModel.IsUpdateMessage = true;

							if (caseModel.prenew_productprice != caseModel.new_productprice)
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

				log.Error($"Error:GetRealsjonInfoOnCreateOrUpdate parsing:  {ex.Message}");
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
				if (data.PostEntityImages[0].value != null)
				{
					if (data.PostEntityImages[0].value.Attributes != null && data.PostEntityImages[0].value.Attributes[0] != null)
					{
						foreach (var att in data.PostEntityImages[0].value.Attributes)
						{
							if (att.key == "ownerid")//Lookup
							{
								if (att.value.Id != null)
									caseModel.ownerid = Convert.ToString(att.value.Id);//	

								if (att.value.Name != null)
									caseModel.ownername = Convert.ToString(att.value.Name);//	in case you need name
							}


							if (att.key == "new_products")// (optionset)
							{
								if (att.value != null && att.value.Value != null)
									if (att.value.Value.Value != null)
										caseModel.new_products = Convert.ToInt32(att.value.Value.Value);//									
							}

							if (att.key == "new_proimageurl")// string (straight)
							{
								if (att.value != null)
									caseModel.new_proimageurl = Convert.ToString(att.value);//									
							}

							if (att.key == "new_productname")// string (straight)
							{
								if (att.value != null)
									caseModel.new_productname = Convert.ToString(att.value);//									
							}

							if (att.key == "new_productprice")//Money
							{
								if (att.value != null)
									caseModel.new_productprice = Convert.ToDouble(att.value.Value);//									
							}
						}
					}
				}

			}//PostEntityImages
			else
			{
				caseModel.new_productname = "";
				caseModel.new_proimageurl = "";
				caseModel.ownername = "";
				casedata.ownerid = "";


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
								if (att.key == "ownerid")//Lookup
								{
									if (att.value.Id != null)
										caseModel.preownerid = Convert.ToString(att.value.Id);//	

									if (att.value.Name != null)
										caseModel.preownername = Convert.ToString(att.value.Name);//	in case you need name
								}


								if (att.key == "new_products")// (optionset)
								{
									if (att.value != null && att.value.Value != null)
										if (att.value.Value.Value != null)
											caseModel.prenew_products = Convert.ToInt32(att.value.Value.Value);//									
								}

								if (att.key == "new_proimageurl")// string (straight)
								{
									if (att.value != null)
										caseModel.prenew_proimageurl = Convert.ToString(att.value);//									
								}

								if (att.key == "new_productname")// string (straight)
								{
									if (att.value != null)
										caseModel.prenew_productname = Convert.ToString(att.value);//									
								}
								if (att.key == "new_productprice")// string (straight)
								{
									if (att.value != null)
										caseModel.prenew_productprice = Convert.ToDouble(att.value.Value);//									
								}
							}
						}
					}


				}
			}//PreEntityImages
			else
			{
				caseModel.preownerid = "";
				caseModel.preownername = "";
				caseModel.prenew_productname = "";
				caseModel.prenew_proimageurl = "";

			}

			// make sure anycase the pre field must be empty not null (avoid exception)
			if (string.IsNullOrWhiteSpace(caseModel.preownerid)) //double check again			
				caseModel.preownerid = "";


			if (string.IsNullOrWhiteSpace(caseModel.preownername)) //double check againin case post delete	
				caseModel.preownername = "";

			if (string.IsNullOrWhiteSpace(caseModel.prenew_productname)) //double check againin case post delete	
				caseModel.prenew_productname = "";

			if (string.IsNullOrWhiteSpace(caseModel.prenew_proimageurl)) //double check againin case post delete	
				caseModel.prenew_proimageurl = "";


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
									if (att.key == "ownerid")//Lookup
									{
										if (att.value.Id != null)
											caseModel.ownerid = Convert.ToString(att.value.Id);//	

										if (att.value.Name != null)
											caseModel.ownername = Convert.ToString(att.value.Name);//	in case you need name
									}


									if (att.key == "new_products")// (optionset)
									{
										if (att.value != null && att.value.Value != null)
											if (att.value.Value.Value != null)
												caseModel.new_products = Convert.ToInt32(att.value.Value.Value);//									
									}

									if (att.key == "new_proimageurl")// string (straight)
									{
										if (att.value != null)
											caseModel.new_proimageurl = Convert.ToString(att.value);//									
									}

									if (att.key == "new_productname")// string (straight)
									{
										if (att.value != null)
											caseModel.new_productname = Convert.ToString(att.value);//									
									}
									if (att.key == "new_productprice")// string (straight)
									{
										if (att.value != null)
											caseModel.new_productprice = Convert.ToDouble(att.value.Value);//									
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
									if (att.key == "ownerid")//Lookup
									{
										if (att.value.Id != null)
											caseModel.ownerid = Convert.ToString(att.value.Id);//	

										if (att.value.Name != null)
											caseModel.ownername = Convert.ToString(att.value.Name);//	in case you need name
									}


									if (att.key == "new_products")// (optionset)
									{
										if (att.value != null && att.value.Value != null)
											if (att.value.Value.Value != null)
												caseModel.new_products = Convert.ToInt32(att.value.Value.Value);//									
									}

									if (att.key == "new_proimageurl")// string (straight)
									{
										if (att.value != null)
											caseModel.new_proimageurl = Convert.ToString(att.value);//									
									}

									if (att.key == "new_productname")// string (straight)
									{
										if (att.value != null)
											caseModel.new_productname = Convert.ToString(att.value);//									
									}
									if (att.key == "new_productprice")// string (straight)
									{
										if (att.value != null)
											caseModel.new_productprice = Convert.ToDouble(att.value.Value);//									
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
