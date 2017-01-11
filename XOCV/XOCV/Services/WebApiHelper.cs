using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;
using XOCV.Helpers;
using XOCV.Interfaces;
using XOCV.Models;
using XOCV.Models.ResponseModels;
using System.Net.Http.Headers;

namespace XOCV.Services
{
	public class WebApiHelper : IWebApiHelper
	{
		private const int DefaultMaxResponseContentBufferSize = 256000;
		//private readonly string BaseUrl = "http://sevan.pilgrimconsulting.com/api";   // LIVE_SERVER
		private readonly string BaseUrl = "http://35.163.41.22/api";                    // TEST_SERVER
		private string UserName { get; set; }
		private int countOfForms { get; set; } = 0;

		private IPictureService PictureService { get; set; }
		private INetworkConnection NetworkConnectionService { get; set; }

		public WebApiHelper()
		{
			NetworkConnectionService = DependencyService.Get<INetworkConnection>();
			PictureService = DependencyService.Get<IPictureService>();
		}

		/// <summary>
		///     Authorization.
		/// </summary>
		/// <param name="login">The login model.</param>
		/// <returns>Getting token for user.</returns>
		public async Task<LoginResponseModel> Authorization(LoginModel login)
		{
			using (var client = new HttpClient())
			{
				try
				{
					client.MaxResponseContentBufferSize = DefaultMaxResponseContentBufferSize;
					UserName = login.UserName;
					var url = new Uri(string.Concat(BaseUrl, "/account/Authenticate"));

					var json = JsonConvert.SerializeObject(login);
					var content = new StringContent(json, Encoding.UTF8, "application/json");

					var response = await client.PostAsync(url, content);

					if (response.IsSuccessStatusCode)
					{
						var result = await response.Content.ReadAsStringAsync();
						var responseModel = JsonConvert.DeserializeObject<LoginResponseModel>(result);
                        UserName = login.UserName;
					    Settings.LocalToken = responseModel.Token;
                        return responseModel;
					}
					throw new Exception(response.ReasonPhrase);
				}
				catch (Exception ex)
				{
					throw new Exception(ex.Message);
				}
			}
		}

		/// <summary>
		///     Get all available moduls and forms.
		/// </summary>
		/// <param name="token">Get moduls with user's token.</param>
		/// <returns>All moduls for current user.</returns>
		public async Task<ContentModel> GetAllContent(string token)
		{
		    if (string.IsNullOrEmpty(token))
		    {
		        throw new Exception("Not Authorized!");
		    }

			if (NetworkConnectionService.IsConnected)
			{
				using (var client = new HttpClient())
				{
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
				    var url = new Uri(string.Concat(BaseUrl, "/getForm"));
				    var response = client.GetAsync(url).Result;
				    if (response.IsSuccessStatusCode)
				    {
				        try
				        {
				            var result = await response.Content.ReadAsStringAsync();
					        var responseModel = JsonConvert.DeserializeObject<ContentModel>(result);

					        #region Additionals

					        foreach (var _form in from company in responseModel.ListOfCompanies from program in company.ListOfPrograms from form in program.SetOfForms from _form in form.Forms select _form)
					        {
						        if (!_form.MultiComplexItems.Any())
						        {
							        foreach (var complexItem in _form.ComplexItems)
							        {
								        _form.MultiComplexItems.Add(new MultiComplexItem { ComplexItems = new List<ComplexItem> { complexItem } });
							        }
						        }

						        foreach (var item in _form.MultiComplexItems.SelectMany(multiComplexItem => multiComplexItem.ComplexItems.SelectMany(complexItem => complexItem.Items)))
						        {
							        item.RadioButtonItemsSource.Insert(0, "");
									if (item.Name == "hintKey")
									{
										if (item.Properties.ImageUrl != null && item.Properties.ImageUrl != "" && item.Properties.ImageUrl != string.Empty)
										{
											string imageName = item.Properties.ImageUrl.Split('/').LastOrDefault();
											if (item.Properties.LocalImagePath != imageName)
											{
												//var imageSource = ImageSource.FromUri(new Uri(item.Properties.ImageUrl));
												item.Properties.LocalImagePath = await PictureService.SaveImageToDisk(item.Properties.ImageUrl, imageName);
											}
										}
									}
						        }
					        }

					        foreach (var form in from company in responseModel.ListOfCompanies from program in company.ListOfPrograms from form in program.SetOfForms select form)
					        {
								countOfForms++;
						        form.Surveyors = new List<SurveyorModel>();

						        var currentSurveyor = new SurveyorModel()
						        {
							        Name = UserName,
							        ComplexFormsModelID = form.ComplexFormsModelID
						        };

						        form.Surveyors.Add(currentSurveyor);
					        }
					        #endregion

					        var db = App.DataBase.GetContent();
					        if (db.Count() == 0)
					        {
						        int i = 1;
						        foreach (var company in responseModel.ListOfCompanies)
						        {
							        foreach (var program in company.ListOfPrograms)
							        {
								        foreach (var form in program.SetOfForms)
								        {
											string formString = JsonConvert.SerializeObject(form);
											App.DataBase.SaveContent(new DBModel { Content = formString, FormID = form.FormId, IsTemplateForm = true, ID = i });
											i++;
								        }
							        }
						        }
					        }
					        else 
					        {
								for (int i = countOfForms; i > 0; i--)
						        {
									try
									{
										App.DataBase.DeleteItem(db.Where(item => item.ID == i).First());
									}
									catch { }
						        }
						        int id = 1;
						        foreach (var company in responseModel.ListOfCompanies)
						        {
							        foreach (var program in company.ListOfPrograms)
							        {
								        foreach (var form in program.SetOfForms)
								        {
									        string formString = JsonConvert.SerializeObject(form);
									        App.DataBase.SaveContent(new DBModel { Content = formString, FormID = form.FormId, IsTemplateForm = true, ID = id });
									        id++;
								        }
							        }
						        }
					        }

					        return responseModel;
				        }
				        catch (Exception e)
				        {
					        var message = e.Message;
				        }
				    }
				    throw new Exception(response.ReasonPhrase);
				}
			}
			else
			{
				var localData = App.DataBase.GetContent().FirstOrDefault();
				var content = JsonConvert.DeserializeObject<ContentModel>(localData.Content);
				return content;
			}
		}

		/// <summary>
		///     Post all filled content from mobile part.
		/// </summary>
		/// <param name="model">Filled model with form.</param>
		/// <returns>True or false. Depends on status code.</returns>
		public async Task<bool> PostAllContent(ProgramModel program)
		{
			if (program == null) return false;

		    var token = Settings.LocalToken;

            if (string.IsNullOrEmpty(token))
		    {
		        throw new Exception("Not authorized!");
		    }

            #region Some strange adding for Radiogroups
            foreach (var complexForm in program.SetOfForms)
			{
				complexForm.StoreNumbers.Clear();
				foreach (var item in from form in complexForm.Forms from multiComplexItem in form.MultiComplexItems from complexItem in multiComplexItem.ComplexItems from item in complexItem.Items select item)
				{
					if (item.RadioButtonItemsSource.Count != 0)
					{
						item.RadioButtonItemsSource.RemoveAt(0);
					}

					if (!string.IsNullOrEmpty(item.Value))
					{
						int parsedNum;
						if (int.TryParse(item.Value, out parsedNum))
						{
							if (parsedNum == -1)
								item.Value = null;
							if (parsedNum > 0)
								item.Value = (parsedNum - 1).ToString();
						}
					}

					if (item.Values.Count > 0)
					{
						for (int i = 0; i < item.Values.Count; i++)
						{
							if (!string.IsNullOrEmpty(item.Values[i]) && item.Values[i] != "")
							{
								int parsedNum;
								if (int.TryParse(item.Values[i], out parsedNum))
								{
									if (parsedNum > 0)
									{
										item.Values[i] = (parsedNum - 1).ToString();
									}
								}
							}
						}
					}
				}
			}
            #endregion

            using (var client = new HttpClient())
			{
			    try
			    {
                    var url = new Uri(string.Concat(BaseUrl, "/AddCapture"));
                    var json = JsonConvert.SerializeObject(program);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                    var response = client.PostAsync(url, content).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var allContent = App.DataBase.GetContent();
                        allContent.RemoveAt(0);
                        var selectedItems = allContent.Where(x => x.IsSelected == true);
                        foreach (var item in selectedItems)
                        {
                            var modelForChanging = JsonConvert.DeserializeObject<ComplexFormsModel>(item.Content);

                            modelForChanging.Captures.First().SyncStatus = Enums.SyncStatus.Sync;
                            item.SyncStatus = Enums.SyncStatus.Sync;
                            item.IsSelected = false;

                            var changedModel = JsonConvert.SerializeObject(modelForChanging);
                            item.Content = changedModel;
                            new Task(() => App.DataBase.SaveContent(item)).Start();
                        }
                        return true;
                    }
                    throw new Exception(response.ReasonPhrase);
                }
                catch (Exception ex)
			    {
			        throw new Exception(ex.Message);
			    }
            }
		}
	}
}