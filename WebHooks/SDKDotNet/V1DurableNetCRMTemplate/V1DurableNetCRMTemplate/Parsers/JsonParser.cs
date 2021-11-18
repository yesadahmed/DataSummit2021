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
			AccountModel accountModel = null;
			try
			{
				dynamic data = JsonConvert.DeserializeObject(ParseJson(webhookJson));//Trim
				if (data != null)
				{
					accountModel = new AccountModel();
					accountModel.PrimaryEntityId = data != null ? data.PrimaryEntityId : "";
					accountModel.PrimaryEntityName = data != null ? data.PrimaryEntityName : "";
					accountModel.MessageName = data != null ? data.MessageName : "";

					if (!accountModel.PrimaryEntityName.Equals("account"))
					{
						log.Error($"Entity is not account.");//just avoid and return task
						return null;
					}

					if (!string.IsNullOrEmpty(accountModel.MessageName))
					{

						if (accountModel.MessageName.ToLower().Equals("create"))
						{
							accountModel = GetAttributesValuesFromPrePostImages(accountModel, data);//  image attributes  only
							accountModel.IsCreateMessage = true;

						}
						else if (accountModel.MessageName.ToLower().Equals("update"))
						{
							accountModel = GetAttributesValuesFromPrePostImages(accountModel, data); //image attributes  only

							if (accountModel.prenew_products != accountModel.new_products)
								accountModel.IsUpdateMessage = true;

							if (!accountModel.prenew_proimageurl.Equals(accountModel.new_proimageurl, StringComparison.OrdinalIgnoreCase))
								accountModel.IsUpdateMessage = true;

							if (accountModel.prenew_productprice != accountModel.new_productprice)
								accountModel.IsUpdateMessage = true;

						}
						else if (accountModel.MessageName.ToLower().Equals("delete"))//image attributes  only
						{
							accountModel = GetAttributesValuesFromPrePostImages(accountModel, data);
							//nothign need here for now
							accountModel.IsDeleteMessage = true;
						}

						if ((data["InputParameters"] != null) && data["InputParameters"].Count != null 
							&& data["InputParameters"].Count > 0) //Define your own attributes in model
						{
							accountModel = GetInputParameterValues(accountModel, data);
						}

						if ((data["OutputParameters"] != null) && data["OutputParameters"].Count != null &&
							data["OutputParameters"].Count > 0) //Define your own attributes in model
						{
							accountModel = GetOutputParametersValues(accountModel, data);
						}
					}
				}
			}
			catch (Exception ex)
			{

				log.Error($"Error:GetInfoOnCreateOrUpdateOrDelete parsing:  {ex.Message}");
			}

			return accountModel;

		}
		#endregion

		#region Private Functions

		private AccountModel GetAttributesValuesFromPrePostImages(AccountModel accountModel, dynamic data)
		{
			AccountModel casedata = accountModel;//copy everything

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
									accountModel.ownerid = Convert.ToString(att.value.Id);//	

								if (att.value.Name != null)
									accountModel.ownername = Convert.ToString(att.value.Name);//	in case you need name
							}


							if (att.key == "new_products")// (optionset)
							{
								if (att.value != null && att.value.Value != null)
									if (att.value.Value.Value != null)
										accountModel.new_products = Convert.ToInt32(att.value.Value.Value);//									
							}

							if (att.key == "new_proimageurl")// string (straight)
							{
								if (att.value != null)
									accountModel.new_proimageurl = Convert.ToString(att.value);//									
							}

							if (att.key == "new_productname")// string (straight)
							{
								if (att.value != null)
									accountModel.new_productname = Convert.ToString(att.value);//									
							}

							if (att.key == "new_productprice")//Money
							{
								if (att.value != null)
									accountModel.new_productprice = Convert.ToDouble(att.value.Value);//									
							}

							if (att.key == "new_userid")//text
							{
								if (att.value != null)
									accountModel.new_userid = Convert.ToString(att.value);//									
							}
						}
					}
				}

			}//PostEntityImages
			else
			{
				accountModel.new_productname = "";
				accountModel.new_proimageurl = "";
				accountModel.ownername = "";
				casedata.ownerid = "";
				accountModel.new_userid = "";

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
										accountModel.preownerid = Convert.ToString(att.value.Id);//	

									if (att.value.Name != null)
										accountModel.preownername = Convert.ToString(att.value.Name);//	in case you need name
								}


								if (att.key == "new_products")// (optionset)
								{
									if (att.value != null && att.value.Value != null)
										if (att.value.Value.Value != null)
											accountModel.prenew_products = Convert.ToInt32(att.value.Value.Value);//									
								}

								if (att.key == "new_proimageurl")// string (straight)
								{
									if (att.value != null)
										accountModel.prenew_proimageurl = Convert.ToString(att.value);//									
								}

								if (att.key == "new_productname")// string (straight)
								{
									if (att.value != null)
										accountModel.prenew_productname = Convert.ToString(att.value);//									
								}
								if (att.key == "new_productprice")// string (straight)
								{
									if (att.value != null)
										accountModel.prenew_productprice = Convert.ToDouble(att.value.Value);//									
								}
								if (att.key == "new_userid")//text
								{
									if (att.value != null)
										accountModel.prenew_userid = Convert.ToString(att.value);//									
								}
							}
						}
					}


				}
			}//PreEntityImages
			else
			{
				accountModel.preownerid = "";
				accountModel.preownername = "";
				accountModel.prenew_productname = "";
				accountModel.prenew_proimageurl = "";
				accountModel.prenew_userid = "";
			}

			// make sure anycase the pre field must be empty not null (avoid exception)
			if (string.IsNullOrWhiteSpace(accountModel.preownerid)) //double check again			
				accountModel.preownerid = "";


			if (string.IsNullOrWhiteSpace(accountModel.preownername)) //double check againin case post delete	
				accountModel.preownername = "";

			if (string.IsNullOrWhiteSpace(accountModel.prenew_productname)) //double check againin case post delete	
				accountModel.prenew_productname = "";

			if (string.IsNullOrWhiteSpace(accountModel.prenew_proimageurl)) //double check againin case post delete	
				accountModel.prenew_proimageurl = "";

			if (string.IsNullOrWhiteSpace(accountModel.prenew_userid)) //double check againin case post delete	
				accountModel.prenew_userid = "";


			return accountModel;
		}


		private AccountModel GetInputParameterValues(AccountModel accountModel, dynamic data)
		{
			AccountModel casedata = accountModel;//copy everything

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
											accountModel.ownerid = Convert.ToString(att.value.Id);//	

										if (att.value.Name != null)
											accountModel.ownername = Convert.ToString(att.value.Name);//	in case you need name
									}


									if (att.key == "new_products")// (optionset)
									{
										if (att.value != null && att.value.Value != null)
											if (att.value.Value.Value != null)
												accountModel.new_products = Convert.ToInt32(att.value.Value.Value);//									
									}

									if (att.key == "new_proimageurl")// string (straight)
									{
										if (att.value != null)
											accountModel.new_proimageurl = Convert.ToString(att.value);//									
									}

									if (att.key == "new_productname")// string (straight)
									{
										if (att.value != null)
											accountModel.new_productname = Convert.ToString(att.value);//									
									}
									if (att.key == "new_productprice")// string (straight)
									{
										if (att.value != null)
											accountModel.new_productprice = Convert.ToDouble(att.value.Value);//									
									}

									if (att.key == "new_userid")//text
									{
										if (att.value != null)
											accountModel.prenew_userid = Convert.ToString(att.value);//									
									}

								}
							}
						}
					}

				}
			}//InputParameters

			return accountModel;
		}


		private AccountModel GetOutputParametersValues(AccountModel accountModel, dynamic data)
		{
			AccountModel casedata = accountModel;//copy everything

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
											accountModel.ownerid = Convert.ToString(att.value.Id);//	

										if (att.value.Name != null)
											accountModel.ownername = Convert.ToString(att.value.Name);//	in case you need name
									}


									if (att.key == "new_products")// (optionset)
									{
										if (att.value != null && att.value.Value != null)
											if (att.value.Value.Value != null)
												accountModel.new_products = Convert.ToInt32(att.value.Value.Value);//									
									}

									if (att.key == "new_proimageurl")// string (straight)
									{
										if (att.value != null)
											accountModel.new_proimageurl = Convert.ToString(att.value);//									
									}

									if (att.key == "new_productname")// string (straight)
									{
										if (att.value != null)
											accountModel.new_productname = Convert.ToString(att.value);//									
									}
									if (att.key == "new_productprice")// string (straight)
									{
										if (att.value != null)
											accountModel.new_productprice = Convert.ToDouble(att.value.Value);//									
									}

									if (att.key == "new_userid")//text
									{
										if (att.value != null)
											accountModel.prenew_userid = Convert.ToString(att.value);//									
									}
								}
							}
						}
					}

				}
			}//OutputParameters


			return accountModel;
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
