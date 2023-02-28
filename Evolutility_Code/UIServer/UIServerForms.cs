//	Copyright (c) 2003-2013 Olivier Giulieri - olivier@evolutility.org 

//	This file is part of Evolutility CRUD Framework.
//	Source link <http://www.evolutility.org/download/download.aspx>

//	Evolutility is open source software: you can redistribute it and/or modify
//	it under the terms of the GNU Affero General Public License as published by
//	the open source software Foundation, either version 3 of the License, or
//	(at your option) any later version.

//	Evolutility is distributed WITHOUT ANY WARRANTY;
//	without even the implied warranty of MERCHANTABILITY
//	or FITNESS FOR A PARTICULAR PURPOSE.
//	See the GNU Affero General Public License for more details.

//	You should have received a copy of the GNU Affero General Public License
//	along with Evolutility. If not, see <http://www.fsf.org/licensing/licenses/agpl-3.0.html>.

//  Commercial license may be purchased at www.evolutility.org <http://www.evolutility.org/product/Purchase.aspx>.


using System;
using System.Text;
using System.Data;
using System.Xml;
using System.Web;
using System.Drawing; 
//using System.Data.SqlClient;

namespace Evolutility
{
	// ==================   HTML generation for all Forms   ==================   

	partial class UIServer 
	{

		public enum EvolDisplayMode
		{
			View = 0,
			Edit = 1,
			NewItem = 2,
			Search = 3,
			AdvancedSearch = 4,
			List = 110,
			Login = 50,
			Selections = 60,
			Export = 70
		}  // to do: ListEdit = 125, SelfHealing = 71, Specifications = 200, Templates = 500, Design = 200 

		private const string sTrue = "True";
		private const string selFieldID = "evoxQSE";
		private const string SelectTagBegin = "<select class=\"Field\" name=\"";

//### Forms ########################################################################################### 
#region "Forms"

		internal string FormEdit(int eDisplayMode)
		{
			/// <summary>EDIT and VIEW forms.</summary>
			int i, iTab, nbTabs = 0, MinLoopXML = 0, MaxLoopXML = 0;
			bool editOK = false, YesNo = false;
			string extraJS = String.Empty;
			int PanelWidth = 0, lineWidth = 0;
			bool inTable = false;
			string TDww = null;
			int totWidth;
			string buffer, buffer2, bufferEnd = null;
			string LinkDetailNew = null;
			bool useTabs = false;
			int nbComments = 0;
			int PanelDetailsIndex = -1, PanelDetailsId = -1;
			string PanelCSSclass, iconBuffer, iconColumnBuffer;
			StringBuilder myHTML = new StringBuilder();
			XmlNodeList aNodeList;

			if (!(ds == null && _ItemID > 0))
			{
				if (!_DBReadOnly && _DBAllowUpdate)
				{
					if (_SecurityModel.Equals(EvolSecurityModel.Multiple_Users_Sharing))
					{
						try
						{
							i = Convert.ToInt32(ds.Tables[0].Rows[0][def_Data.dbcolumnuserid]);
						}
						catch
						{
							i = -10;
						}
					}
					else
						i = _UserID;
					if (i != _UserID && _ItemID > 0)
					{
						eDisplayMode = 0;
						_DisplayMode = 0;
					}
					else
						editOK = true;
				}
				iconBuffer = icon;
				iconColumnBuffer = dbcolumnicon;
				//##### HEADER ##### 
				XmlNodeList mes = myDOM.DocumentElement.ChildNodes;
				int Holder = 0;
				//back to last search result 
				if (Page.Cache[String.Format("{0}_W", GetCacheKey(def_Data.dbtable))] != null)
					myHTML.AppendFormat("&nbsp;<a href=\"javascript:EvPost('105')\" class=\"evo-ml\">{0}</a>", EvoLang.Back2SearchResults);
				//for (var i=0;i<mes.Count;i++)
				foreach (XmlNode me in mes)
				{
					if (me.Name == xElement.tab)
					{
						Holder = 2;
						break;
					}
					if (me.Name == xElement.panel)
					{
						if (me.NextSibling != null && me.NextSibling.Name == xElement.tab)
						{
							Holder = 1;
							myHTML.Append(HTMLPanelFields(me, eDisplayMode, "0p", _ItemID > 0, 0));
						}
						break;
					}
				}
				//--- Tabs 
				int activeTab = 0;
				XmlNodeList aNodeListTabs = null;
				if (Holder > 0)
					aNodeListTabs = myDOM.DocumentElement.SelectNodes(xQuery.tab, nsManager);
				if (aNodeListTabs != null)
				{
					nbTabs = aNodeListTabs.Count;
					useTabs = nbTabs > 0;
					if (useTabs)
					{
						activeTab = EvoTC.String2Int(Page.Request["EvoActTab"]);
						//-- Javascript for hidding and showing tabs 
						extraJS = string.Format("var TABevo={0};", activeTab);
					}
					else
						nbTabs = 1;
				}
				else
					nbTabs = 1;
				for (iTab = 0; iTab < nbTabs; iTab++)
				{
					if (useTabs)
					{
						//tabs buttons 
						aNodeList = aNodeListTabs[iTab].ChildNodes;
						if (iTab == 0)
							myHTML.Append(HTMLTabList(aNodeListTabs, activeTab, nbTabs, _TabPosition));
						//begin tab 
						myHTML.AppendFormat("<div id=\"{0}Tab{1}\"{2}>", "z", iTab, EvoUI.StyleVisibleToggle(iTab == activeTab));
					}
					else
						aNodeList = myDOM.DocumentElement.ChildNodes;
					inTable = false;
					lineWidth = 0;
					MaxLoopXML = aNodeList.Count - 1;
					//###--- Panels & PanelsDetails 
					if (MaxLoopXML > 0 && aNodeList[0].Name == xElement.data)
						MinLoopXML = 1;
					for (i = MinLoopXML; i <= MaxLoopXML; i++)
					{
						XmlNode cn = aNodeList[i];
						buffer = cn.Name;
						if (buffer.StartsWith(xElement.panel))
						{
							if (cn.Attributes[xAttribute.width] == null)
								PanelWidth = 100;
							else
							{
								PanelWidth = EvoTC.String2Int(cn.Attributes[xAttribute.width].Value);
								if (PanelWidth == 0)
									PanelWidth = 100;
							}
							TDww = string.Format("<td width=\"{0}%\">", PanelWidth.ToString());
							buffer2 = string.Format("{0}p", (iTab + 1) * 100 + i);
							totWidth = lineWidth + PanelWidth;
							switch (totWidth)
							{
								case 100:
									if (PanelWidth < 100)
									{
										myHTML.Append(TDww);
										inTable = false;
									}
									else
										myHTML.Append(tableBeginTr).Append(TDww);
									bufferEnd = TdTrTableEnd;
									lineWidth = 0;
									break;
								default:
									if (totWidth > 100)
									{
										if (inTable) myHTML.Append(trTableEnd);
										myHTML.Append(tableBeginTr).Append(TDww);
										bufferEnd = "</td>";
										inTable = true;
										lineWidth = PanelWidth;
									}
									else
									{
										if (lineWidth == 0)
										{
											if (inTable)
												myHTML.Append(trTableEnd);
											myHTML.Append(tableBeginTr);
											inTable = true;
										}
										myHTML.Append(TDww);
										bufferEnd = "</td>";
										lineWidth = lineWidth + PanelWidth;
									}
									break;
							}
							if (buffer == xElement.panel)
							{      //##--- panel 
								myHTML.Append(HTMLPanelFields(cn, eDisplayMode, buffer2, _ItemID > 0, i - MinLoopXML + Holder * 100));
							}
							else
							{      //##--- panel details 
								if (!detailsLoaded)
									BuildSQLDetailsAndComments(true);
								PanelDetailsIndex += 1;
								{
									if (cn.Attributes["panelid"] == null)
										PanelDetailsId = PanelDetailsIndex;
									else
										PanelDetailsId = EvoTC.String2Int(cn.Attributes["panelid"].Value);
									if (cn.Attributes["linknew"] == null)
										LinkDetailNew = string.Empty;
									else
										LinkDetailNew = cn.Attributes["linknew"].Value;
									if (cn.Attributes[xAttribute.icon] == null)
										icon = string.Empty;
									else
										icon = EvoUI.HTMLIcon(_PathPixToolbar, cn.Attributes[xAttribute.icon].Value);
									buffer = cn.Attributes[xAttribute.label].Value;
									if (cn.Attributes[xAttribute.cssClass] == null)
										PanelCSSclass = ""; //EvoUI.cssPanel;
									else
									{
										PanelCSSclass = cn.Attributes[xAttribute.cssClass].Value;
										//if (string.IsNullOrEmpty(PanelCSSclass))
										//	PanelCSSclass = EvoUI.cssPanel;
									}
									myHTML.Append(EvoUI.HTMLDiv(buffer2, true));
									if (ds2 != null)
										YesNo = ds2.Tables[PanelDetailsIndex].Rows.Count > 0;
									if (YesNo | (_DBAllowInsertDetails & _DisplayMode == 1 & !_DBReadOnly))
									{
										myHTML.AppendFormat("<table class=\"{0} Holder\"><tr><td>", PanelCSSclass);
										string panelId = buffer2 + "dtl";
										if (!string.IsNullOrEmpty(buffer))
										{
											myHTML.Append(EvoUI.HTMLPanelLabel(buffer, panelId, EvoUI.cssPanelLabel, _CollapsiblePanels, IEbrowser))
												.Append(EvoUI.tag_cTDcTRoTRoTD);
										}
										if (cn.Attributes[xAttribute.icon] == null)
											icon = string.Empty;
										else
											icon = cn.Attributes[xAttribute.icon].Value;
										if (cn.Attributes[xAttribute.dbColumnIcon] == null)
											dbcolumnicon = string.Empty;
										else
											dbcolumnicon = cn.Attributes[xAttribute.dbColumnIcon].Value;
										myHTML.AppendFormat("<span id=\"{0}\">", panelId)
											.Append(HTMLDataset(cn.ChildNodes, String.Empty, buffer, eDisplayMode + 1, LinkDetailNew, false, PanelDetailsIndex, PanelDetailsId))
											.Append("</span>");
										myHTML.Append(TdTrTableEnd);
									}
								}
								myHTML.Append("</div>");
							}
							myHTML.Append(bufferEnd);
						}
					}
					if (i > 0 && inTable)
						myHTML.Append(trTableEnd);
					//close tabs 
					if (useTabs)
					{
						myHTML.Append("</div>");
						if (iTab > nbTabs - 2)
							break;
					}
					else
						break;
				}
				if (useTabs && _TabPosition != EvolTabPosition.Top)
					myHTML.Append(TdTrTableEnd);
				//##### BUTTONS ##### 'Save, Save and Add, Cancel / Edit ...and Next Last... 
				myHTML.Append(FormButtons(eDisplayMode, editOK));
				if (_DisplayMode == 0 && _ItemID > 0)
				{
					//##### COMMENTS ##### 
					if (_DBAllowComments != EvolCommentsMode.None)
					{
						try
						{
							nbComments = Convert.ToInt32(ds.Tables[0].Rows[0][SQLColNbComments]);
						}
						catch
						{
							nbComments = 0;
							noCommentsHere = true;
						}
						if (!detailsLoaded)
							BuildSQLDetailsAndComments(true);
						myHTML.Append(FormComments(nbComments));
						//'##### RATING ##### 
						//If _DBAllowRating Then 
						// If Not atRunTime Then 
						// nbRating = CInt(ds.Tables[0].Rows(0)("maNbRatings")) 
						// myHTML.Append(nbRating & "Ratings") 
						// If nbComments > 0 Then 
						// sql = EvoDB.BuildSQL("*", "ka_Comments", "table='k_contact' and itemid=" & _ItemID, "macdate DESC") 
						// myHTML.Append(HTMLDataset(aNodeList, sql, String.Empty, 0)) 
						// Else 

						// End If 
						// End If 
						//End If 
					}
				}
				icon = iconBuffer;
				dbcolumnicon = iconColumnBuffer;
				myHTML.Append(HTMLNavLinks());
			}
			else
				myHTML.Append("<p>&nbsp;").Append(EvoLang.err_NoDataDisp).Append("<br/>&nbsp;</p>");
			if (extraJS.Length > 0)
				JSWrite(extraJS);
			if (detailsLoaded)
			{
				ds2 = null;
				detailsLoaded = false;
				myHTML.Append(EvoUI.HTMLInputHidden("evoUDtls", ""));
			}
			return myHTML.ToString();
		}
		
		internal string FormSearch(int sDisplayMode)
		{
			/// <summary>SEARCH and ADVANCED SEARCH forms.</summary>
			string SelectOptionsNull = string.Format(EvoTC.f_0_1, EvoUI.HTMLOption(EvoDB.soIsNull, EvoLang.sIsNull), EvoUI.HTMLOption(EvoDB.soIsNotNull, EvoLang.sIsNotNull));
			string fType, fLabel, fID, buffer;
			int fDisplayMode = 0;
			StringBuilder myHTML = new StringBuilder();
			string R2Lcss = EvoLang.R2L?" Right":String.Empty; //css is not "R2L" this time

			_ItemID = 0;
			if (sDisplayMode == 3)
				buffer = xQuery.qPanelFields(xAttribute.search);
			else
				buffer = xQuery.qPanelFields(xAttribute.searchAdv);
			XmlNodeList aNodeList = myDOM.DocumentElement.SelectNodes(buffer, nsManager);
			string myCondition = EvoUI.HTMLSearchOperators(EvoLang.sEquals, EvoLang.sStart, EvoLang.sContain, EvoLang.sFinish);
			myHTML.Append("<br/><table class=\"FormSearch\" id=\"GridSearch\">");
			foreach (XmlNode aNode in aNodeList)
			{
				 myHTML.Append("<tr><td width=\"20%\" class=\"SearchLabel").Append(R2Lcss).Append("\">");
				fType = aNode.Attributes[xAttribute.type].Value;
				fLabel = xAttribute.GetFieldLabel(aNode);
				if (_ShowDesigner)
					fLabel += EvoUI.LinkDesigner(EvoUI.DesType.fld, Convert.ToInt32(aNode.Attributes[xAttribute.id].Value), fLabel, _PathDesign);
				//BUG maybe can check that each field is only there once 
				fID = UID + aNode.Attributes[xAttribute.dbColumn].Value;
				myHTML.Append(EvoUI.HTMLFieldLabelSpan(fID, fLabel)).Append("</td>");
				//advanced search 
				if (sDisplayMode == 4)
				{
					switch (fType)
					{
						case EvoDB.t_lov:
							myHTML.Append("<td width=\"25%\"><div class=\"FieldReadOnly\">")
								.Append(EvoLang.anyof)
								.Append("</div></td><td width=\"55%\">");
							break;
						case EvoDB.t_bool:
						case EvoDB.t_pix:
						case EvoDB.t_doc:
							myHTML.Append("<td width=\"80%\" colspan=\"2\">");
							break;
						default:
							myHTML.Append("<td width=\"25%\">")
								.Append(SelectTagBegin).Append(fID);
							switch (fType)
							{
								case EvoDB.t_date:
								case EvoDB.t_datetime:
								case EvoDB.t_time:
									myHTML.Append("_c\">");
									if (fType.Equals(EvoDB.t_time))
										myHTML.Append(EvoUI.HTMLOption(EvoDB.soEqual, EvoLang.cAt));
									else
										myHTML.Append(EvoUI.HTMLOption(EvoDB.soEqual, EvoLang.sOn));
									myHTML.Append(EvoUI.HTMLOption(EvoDB.soGreaterThan, EvoLang.sAfter)).Append(EvoUI.HTMLOption(EvoDB.soSmallerThan, EvoLang.sBefore));
									break;
								case EvoDB.t_dec:
								case EvoDB.t_int:
									myHTML.Append("_c\">").Append(EvoUI.HTMLOption(EvoDB.soEqual, "&#61;")).Append(EvoUI.HTMLOption(EvoDB.soGreaterThan, "&#62;", true)).Append(EvoUI.HTMLOption(EvoDB.soSmallerThan, "&#60;"));
									break;
								default:
									myHTML.Append(myCondition);
									break;
							}
							if (aNode.Attributes[xAttribute.required] == null || (aNode.Attributes[xAttribute.required] != null && aNode.Attributes[xAttribute.required].Value != s1))
								myHTML.Append(SelectOptionsNull);
							myHTML.Append("</select></td><td width=\"55%\">");
							break;
					}
				}
				//regular search 
				else
					myHTML.Append("<td width=\"80%\">");
				//all search 
				switch (fType)
				{
					case EvoDB.t_text:
					case EvoDB.t_txtm:
					case EvoDB.t_html:
					case EvoDB.t_email:
					case EvoDB.t_url:
						myHTML.Append(EvoUI.HTMLInputTextEmpty(fID));
						break;
					case EvoDB.t_lov:
						if (aNode.Attributes[xAttribute.dbColumnIcon] == null)
							fLabel = string.Empty;
						else
							fLabel = aNode.Attributes[xAttribute.dbColumnIcon].Value;
						if (string.IsNullOrEmpty(fLabel))
							fDisplayMode = sDisplayMode;
						else
							fDisplayMode = 4;
						if (fDisplayMode == 3) // search
							myHTML.Append(SelectTagBegin).Append(fID).Append("\" onclick=\"javascript:Evol.addFldLabel(this,0)\"><option value=\"\" selected>- ").Append(EvoLang.any).Append(" -</option>").Append(HTMLlov(aNode, String.Empty, s0, LOVFormat.HTML, 0)).Append("</select>");
						else // adv. search
							myHTML.Append(HTMLlov(aNode, fID, s0, LOVFormat.HTML, 0));
						break;
					case EvoDB.t_bool:
						myHTML.Append(EvoUI.HTMLInputRadio(fID, String.Empty, EvoLang.any, true, fID + "-"))
							.Append(EvoUI.HTMLInputRadio(fID, s1, EvoLang.yes, false, fID + s1))
							.Append(EvoUI.HTMLInputRadio(fID, s0, EvoLang.no, false, fID + s0));
						break;
					case EvoDB.t_date:
					case EvoDB.t_datetime:
					case EvoDB.t_time:
						//advanced search 
						if (sDisplayMode.Equals(4))
							myHTML.Append(EvoUI.HTMLInputDate(fID, String.Empty, _Language, _PathPixToolbar));
						//regular search 
						else
						{
							myHTML.Append("<nobr>").Append(SelectTagBegin).Append(fID).Append("dir\" style=\"width:50%\"><option value=\"\">")
								.Append(EvoLang.sDateRangeWithin).Append("</option><option value=\"P\">").Append(EvoLang.sDateRangeLast)
								.Append("</option><option value=\"F\">").Append(EvoLang.sDateRangeNext).Append("</option></select>")
								.Append(SelectTagBegin).Append(fID).Append("\" style=\"width:50%\"><option value=\"\">").Append(EvoLang.sDateRangeAny).Append("</option>")
								.Append(EvoUI.HTMLlovEnum(EvoLang.sDateRange, String.Empty, 0, false)).Append("</select></nobr>");
						}
						break;
					//Case EvoDB.t_int 
					case EvoDB.t_pix:
						myHTML.Append(EvoUI.HTMLInputCheckBox(fID, s1, EvoLang.wPix, false, fID));
						break;
					case EvoDB.t_doc:
						myHTML.Append(EvoUI.HTMLInputCheckBox(fID, s1, EvoLang.wDoc, false, fID));
						break;
					case EvoDB.t_int:
					case EvoDB.t_dec:
						myHTML.Append("<input class=\"Field\" type=\"text\" name=\"").AppendFormat("{0}\" id=\"{0}", fID)
							.Append("\" OnKeyUp=\"EvoVal.checkNum(this,'").Append(fType.Substring(0, 1)).Append("')\">");
						break;
					default:
						myHTML.Append(EvoUI.HTMLInputTextEmpty(buffer));
						break;
				}
				myHTML.Append("</td></tr>");
			}
			//'multiusers 
			//if (_SecurityMode.Equals(EvolSecurityMode.Multiple_Users_Sharing)) { 
			//    myHTML.Append("<tr valign=\"top\"><td width=\"20%\"><p align=\"right\">"); 
			//    myHTML.Append(EvoUI.HTMLFieldLabelSpan("", "Owner", "font", String.Empty)).Append("</p></td><td width=\""); 
			//    if (sDisplayMode.Equals(4)) { 
			//        myHTML.Append("20%\"></td><td width=\"60%\">"); 
			//    } 
			//    else { 
			//        myHTML.Append("80%\">"); 
			//    } 
			//    buffer = UID + "OWNER"; 
			//    myHTML.Append(EvoUI.HTMLInputRadio(buffer, s1, EvoLang.MyEntities.Replace("~ENTITIES~", def_Data.entities), false, buffer + s1)).Append(EvoUI.HTMLSpace); 
			//    myHTML.Append(EvoUI.HTMLInputRadio(buffer, String.Empty, EvoLang.PubMine, true, buffer + s0)).Append(EvoUI.HTMLSpace); 
			//    myHTML.Append("</td></tr>"); 
			//} 
			//With comments search box 
			if (_DBAllowComments != EvolCommentsMode.None)
			{
				myHTML.Append("<tr><td><p class=\"Right\">");
				myHTML.Append(EvoUI.HTMLFieldLabelSpan(EvoLang.wComments, EvoUI.HTMLPixComment)).Append("</p></td><td>");
				if (sDisplayMode.Equals(4))
					myHTML.Append("</td><td>");
				myHTML.Append(EvoUI.HTMLInputCheckBox("EvoxUCM", s1, EvoLang.wComments, false, "EVOxucm"));
				myHTML.Append("</td></tr>");
			}
			myHTML.Append("</table><br/>");
			// button
			myHTML.Append("<div class=\"PanelButtons\"><input type=\"submit\" name=\"Search\" value=\" ");
			myHTML.Append(EvoLang.Search).Append(" \" class=\"IndentBtn Button\">");
			if (sDisplayMode == 3)
			{
				myHTML.Append(HTMLSpace3).Append(EvoUI.HTMLLinkEventRef(s4, EvoLang.AdvSearch));
			}
			myHTML.Append("</div>");

			return myHTML.ToString();
		}

		internal string FormMassUpdate()
		{
			/// <summary>MASS UPDATE form for field of types lov, boolean that are not readonly.</summary>
			string fType, fLabel, fID; 
			StringBuilder myHTML = new StringBuilder();
			string R2Lcss = EvoLang.R2L ? " Right" : String.Empty; //css is not "R2L" this time

			_ItemID = 0;
			XmlNodeList aNodeList = myDOM.DocumentElement.SelectNodes(xQuery.aggregableFields, nsManager);
			myHTML.Append("<table class=\"FormSearch\"><tr><td colspan=\"2\">") // re-use CSS class name "FormSearch"
				.Append(EvoUI.HTMLFieldLabelSpan(selFieldID, EvoLang.Selection))
				.Append(LastSearchCondition(false))
				.Append("</td></tr>");
			foreach (XmlNode aNode in aNodeList)
			{
				bool showField = true;
				if (aNode.Attributes[xAttribute.dbReadOnly] != null)
					showField = aNode.Attributes[xAttribute.dbReadOnly].Value == "0";
				if (showField)
				{
					myHTML.Append("<tr><td width=\"20%\" class=\"SearchLabel").Append(R2Lcss).Append("\">");
					fType = aNode.Attributes[xAttribute.type].Value;
					fLabel = xAttribute.GetFieldLabel(aNode);
					//if (_ShowDesigner)
					//    fLabel += EvoUI.LinkDesigner(EvoUI.DesType.fld, Convert.ToInt32(aNode.Attributes[xAttribute.id].Value), fLabel, _PathDesign);
					fID = UID + aNode.Attributes[xAttribute.dbColumn].Value;
					myHTML.Append(EvoUI.HTMLFieldLabelSpan(fID, fLabel)).Append("</td>")
						.Append("<td width=\"80%\">")
						.Append(SelectTagBegin).Append(fID)
						.Append("\" onclick=\"javascript:Evol.addFldLabel(this,0)\"><option value=\"\" selected>- ")
						.Append(EvoLang.NoChange).Append(" -</option>");
					if (fType == EvoDB.t_bool)
					{
						myHTML.Append(EvoUI.HTMLOption("Y", EvoLang.yes))
							.Append(EvoUI.HTMLOption("N", EvoLang.no));
					}
					else //if (fType == EvoDB.t_lov)
					{
						bool allowNulls = true;
						if (aNode.Attributes[xAttribute.required] != null)
							allowNulls = aNode.Attributes[xAttribute.required].Value == "0";
						if (allowNulls)
							myHTML.AppendFormat("<option value=\"null\">{0}</option>", string.Format(EvoLang.NoX, fLabel));
						myHTML.Append(HTMLlov(aNode, String.Empty, s0, LOVFormat.HTML, 0));
					}
					myHTML.Append("</select></td></tr>");
				}
			}
			myHTML.Append("</table><br/>")
				.Append("<div class=\"PanelButtons\"><input type=\"submit\" name=\"MU\" value=\" ")
				.Append(EvoLang.MassUpdate).Append(" \" class=\"IndentBtn Button\">")
				.Append("</div>");
			return myHTML.ToString();
		}

		internal string FormList(int lDisplayMode)
		{
			/// <summary>LIST form (used for list all + search results + selections).</summary>
			string[] sql = BuildSQLlist(lDisplayMode);
			return HTMLDataset(myDOM.DocumentElement.SelectNodes(xQuery.qPanelFields(xAttribute.searchList), nsManager), sql[0], sql[1], 0, String.Empty, true, 0, -1);
		}

		internal string FormExport()
		{
			/// <summary>EXPORT form (option panels are generated in Javascript).</summary>
			string fieldName, fieldlabel, expOut, buffer;
			StringBuilder myHTML = new StringBuilder();
			char[] sepChars = { '-' };

			expOut = xptCSV;
			XmlNodeList aNodeList = myDOM.DocumentElement.SelectNodes(xQuery.panelField, nsManager);
			myHTML.Append("<table class=\"FormExport\"><tr><td width=\"62%\">"); // table = 2 columns
			myHTML.Append(EvoUI.HTMLFieldLabel(selFieldID, EvoLang.Selection))
				.Append(LastSearchCondition(true));
			//##### export format ######################################## 
			string[] myLabels = EvoLang.ExportFormats.Split(sepChars);
			myHTML.Append(EvoUI.HTMLFieldLabel("evoZOut", EvoLang.ExportFormat))
				.Append("<select class=\"FieldReadOnly\" name=\"evoZOut\" onChange=\"EvoExport.showFormatOpts(this.value)\">")
				.Append(EvoUI.HTMLOption(xptCSV, myLabels[0], true))
				.Append(EvoUI.HTMLOption(xptHTML, myLabels[1]))
				.Append(EvoUI.HTMLOption(xptSQL, myLabels[2]))
				.Append(EvoUI.HTMLOption(xptTAB, myLabels[3]))
				.Append(EvoUI.HTMLOption(xptXML, myLabels[4]))
				.Append(EvoUI.HTMLOption(xptJSON, myLabels[5]))
				.Append("</select>\n");
			myHTML.Append("<div class=\"ExportOptions\">\n");
			//##### CSV, TAB - First line for field names ####### 
			myHTML.Append(EvoUI.HTMLDiv(UID + xptCSV, (expOut == xptCSV || expOut == xptTAB)));
			//# field - header ####### 
			buffer = UID + "FLH";
			myHTML.Append(EvoUI.HTMLFieldLabel(buffer, EvoLang.ExportHeader))
				.Append(EvoUI.HTMLInputCheckBox(buffer, s1, EvoLang.ExportFirstLine, true, buffer));
			//# field - separator 
			//# - csv - any separator ####### 
			myHTML.Append("<br/><br/><div id=\"").Append(UID).Append("csv2\"").Append(EvoUI.StyleVisibleToggle(expOut == xptCSV)).Append(">");
			fieldName = "FLS_evol";
			myHTML.Append(EvoUI.HTMLFieldLabel(fieldName, EvoLang.ExportSeparator))
				.Append(EvoUI.HTMLInputText(fieldName, GetPageRequest(fieldName, coma), 5));
			myHTML.Append("</div><div id=\"").Append(UID).Append("tab2\"").Append(EvoUI.StyleVisibleToggle(expOut == xptTAB)).Append(">");
			//# - tab - hardcoded tab #######
			myHTML.Append(EvoUI.HTMLFieldLabel("", EvoLang.ExportSeparator)).Append("TAB</div></div>");
			//# XML - Root element name #######
			myHTML.Append(EvoUI.HTMLDivClosed(UID + xptXML, expOut == xptXML));
			//# HTML - Header color + Color odd rows + Color even rows ####### 
			myHTML.Append(EvoUI.HTMLDivClosed(UID + xptHTML, expOut == xptHTML));
			//# SQL - transaction ####### 
			myHTML.Append(EvoUI.HTMLDivClosed(UID + xptSQL, expOut == xptSQL));
			myHTML.Append("\n</div>");
			myHTML.Append("</td><td width=\"38%\">");
			//### list of columns to export ######################################### 
			myHTML.Append(EvoUI.HTMLFieldLabel(string.Empty, EvoLang.ExportFields))
				.Append(EvoUI.HTMLInputCheckBox("showID", s1, EvoLang.IDkey, false, "showID"))
				.Append(EvoUI.tag_BR);
			int maxLoopXML = aNodeList.Count;
			bool inDiv = false;
			for (int i = 0; i < maxLoopXML; i++)
			{
				XmlNode cn = aNodeList[i];
				fieldlabel = xAttribute.GetFieldLabel(cn);
				buffer = cn.Attributes[xAttribute.dbColumn].Value;
				if (string.IsNullOrEmpty(fieldlabel))
					fieldlabel = buffer;
				myHTML.Append(EvoUI.HTMLInputCheckBox(UID + buffer, s1, fieldlabel, true, buffer + i.ToString())).Append(EvoUI.tag_BR);
				if (i == 12 && maxLoopXML > 16)
				{
					myHTML.Append(EvoUI.HTMLLinkShowVanish("EVOmfld", EvoLang.AllFields))
						.Append("<div id=\"EVOmfld\" style=\"display:none\">");
					inDiv = true;
				}
			}
			if (inDiv)
				myHTML.Append("</div>");
			myHTML.Append(TdTrTableEnd);
			// Download button
			myHTML.Append("<div class=\"PanelButtons\">")
				.Append("<input type=\"button\" class=\"IndentBtn Button\" onclick=\"Javascript:EvPost('71')\" name=\"XP\" value=\" ")
				.AppendFormat(EvoLang.DownloadEntity, def_Data.entities).Append(" \">")
				.Append("</div>");
			myLabels = null;
			return myHTML.ToString();
		}

		internal string FormQueries()
		{
			/// <summary>SELECTIONS (canned queries) form.</summary>
			StringBuilder myHTML = new StringBuilder();

			myHTML.Append("<div class=\"Holder\">");
			if (!_DBAllowSelections)
				myHTML.Append(EvoUI.HTMLMessage(EvoLang.warn_NoAccessSelections, EvoUI.MsgType.Warn));
			else
			{
				XmlNode cn = myDOM.DocumentElement.SelectSingleNode(xQuery.queries, nsManager);
				if (cn == null)
					myHTML.Append(EvoUI.HTMLMessage(EvoLang.warn_NoQueryDef, EvoUI.MsgType.Warn));
				else
				{
					if (cn.Attributes["header"] != null)
					{
						string header = cn.Attributes["header"].Value;
						if (header != string.Empty)
							myHTML.Append("<p>&nbsp;&nbsp;").Append(header).Append("</p>");
					}
					XmlNodeList aNodeList = cn.ChildNodes;
					if (aNodeList.Count < 1)
						myHTML.Append("<p>No queries defined.</p>");
					else
					{
						myHTML.Append("<ul class=\"Queries\">");
						foreach (XmlNode aNode in aNodeList)
						{
							if (aNode.NodeType == XmlNodeType.Element && aNode.Attributes[xAttribute.url] != null && aNode.Attributes[xAttribute.label] != null)
								myHTML.Append("<li>").Append(EvoUI.HTMLLinkEventRef("q:" + aNode.Attributes[xAttribute.url].Value, aNode.Attributes[xAttribute.label].Value)).Append("</li>");
						}
						myHTML.Append("</ul>");
					}
				}
			}
			myHTML.Append("</div>");
			return myHTML.ToString();
		}

		internal string FormCharts()
		{
			/// <summary>CHARTS form for dashboard.</summary>
			///   

			string fType, fLabel, fID;
			StringBuilder myHTML = new StringBuilder();
			string R2Lcss = EvoLang.R2L ? " Right" : String.Empty; //css is not "R2L" this time
			const string beginImgLink = "<img src=\"http://chart.apis.google.com/chart?";
			const string hb = "<div class=\"ChartHolder\">", he = "</div>";
			//int lovNum = 0;

			string ErrorMsg = "";
			DataSet ds;
			StringBuilder sql = new StringBuilder();
  
			_ItemID = 0;
			XmlNodeList aNodeList = myDOM.DocumentElement.SelectNodes(xQuery.aggregableFields, nsManager);
			if (aNodeList.Count > 0)
			{
				// generate SQL
				string dbTable = def_Data.dbtable;
				string sqlWhere = "";
				if (_SecurityModel.Equals(EvolSecurityModel.Multiple_Users_RLS) || _SecurityModel.Equals(EvolSecurityModel.Multiple_Users_Sharing))
					sqlWhere += string.Format(" AND {0}={1} ", def_Data.dbcolumnuserid, _UserID);

				foreach (XmlNode aNode in aNodeList)
				{
					bool showField = true;
					if (aNode.Attributes[xAttribute.dbReadOnly] != null)
						showField = aNode.Attributes[xAttribute.dbReadOnly].Value == "0";
					if (showField)
					{
						fID = aNode.Attributes[xAttribute.dbColumn].Value;
						fType = aNode.Attributes[xAttribute.type].Value;
						switch (fType)
						{
							case EvoDB.t_bool:
								sql.Append(EvoDB.BuildSQL(
									fID + ", count(*)",
									def_Data.dbtable,
									sqlWhere,
									def_Data.dbtable + "." + fID,
									"",
									fID + " desc",
									20));
								break;
							case EvoDB.t_lov:
								string dbTableLOV = aNode.Attributes[xAttribute.dbTableLOV].Value;
								string dbTableColumnLOV = dbTableLOV + "." + ((aNode.Attributes[xAttribute.dbColumnReadLOV] != null) ? aNode.Attributes[xAttribute.dbColumnReadLOV].Value : "name");
								sql.Append(EvoDB.BuildSQL(
									dbTableColumnLOV + ", count(*)",
									dbTable + "," + dbTableLOV,
									string.Format("{0}.{1}={2}.ID", dbTable, fID, dbTableLOV), 
									dbTableColumnLOV + ((sqlWhere.Length > 0) ? " AND " + sqlWhere : ""),
									"",
									dbTableColumnLOV + " desc", //"count(*) desc",
									20));
								break;
						}
					}
				}
				ds = EvoDB.GetData(sql.ToString(), SqlConnection, ref ErrorMsg);
				if (ds != null && ds.Tables != null)
				{
					// generate HTML
					for (int i = 0; i < aNodeList.Count; i++)
					{
						XmlNode aNode = aNodeList[i];
						bool showField = true;
						if (aNode.Attributes[xAttribute.dbReadOnly] != null)
							showField = aNode.Attributes[xAttribute.dbReadOnly].Value == "0";
						if (showField)
						{
							fID = aNode.Attributes[xAttribute.dbColumn].Value;
							fType = aNode.Attributes[xAttribute.type].Value;
							if (aNode != null && ds != null && ds.Tables.Count > i)
							{		
								fLabel = xAttribute.GetFieldLabel(aNode);
								DataTable dt = ds.Tables[i];
								switch (fType)
								{
									case EvoDB.t_bool:
										//chco=4D89F9,C6D9FD 
										int yesCount;
										if (dt.Rows.Count > 1)
										{
											yesCount = EvoTC.String2Int(dt.Rows[1][1].ToString());
										}
										else
										{
											yesCount = 0;
										}
										int noCount = EvoTC.String2Int(dt.Rows[0][1].ToString());
										int maxCount = (yesCount >= noCount) ? yesCount : noCount;
										myHTML.Append(hb).Append(chartTitle(fLabel))
											.Append(beginImgLink).Append("chbh=a&amp;chs=300x225&cht=bvg&chco=A2C180,FF9900&chds=0,")
											.Append(maxCount).Append("&chd=t:").Append(yesCount).Append("|").Append(noCount)
											.Append("&chp=0.05&chts=676767,10.5&chdl=Yes (").Append(yesCount).Append(")|No (")
											.Append(noCount).Append(")").Append("\"><br/>");
										// + "&chl=Yes (" + yesCount + ")|No (" + noCount + ")
										//&chtt=title+chart
										myHTML.Append(he);
										break;
									case EvoDB.t_lov:
										string chd = "";
										string chl = "";
										for (int r = 0; r < dt.Rows.Count; r++)
										{
											int v = EvoTC.String2Int(dt.Rows[r][1].ToString());
											chd += v + ",";
											chl += HttpUtility.UrlEncode(dt.Rows[r][0].ToString()) + " (" + v + ")|";
										}
										if (chd.Length > 0)
										{
											chd = chd.Substring(0, chd.Length - 1);
										}
										if (chl.Length > 0)
										{
											chl = HttpUtility.HtmlEncode(chl.Substring(0, chl.Length - 1));
										}
										myHTML.Append(hb).Append(chartTitle(String.Format(EvoLang.chart_A_per_B, EvoTC.ToUpperLowers(def_Data.entities), fLabel)));
										//if(lovNum==0){
										//    lovNum++;
										myHTML.Append(beginImgLink).AppendFormat("chd=t:{0}&chl={1}", chd, chl).Append("&cht=p&chds=0,20&chs=400x200\"><br/>").Append(he);
										//}else{
										//    myHTML.Append(hb).Append("<img src=\"http://chart.apis.google.com/chart?cht=p&chco=BBBB00,BB0000&chd=t:4,5,1,2&chs=400x200&chl=a|b|c|d\"><br/>");
										//  //chco=FFFF10,FF0000
										//}
										break;
								}
							}
							else
							{
								myHTML.Append(hb).Append(EvoLang.NoGraph).Append(he);
							} 
						}
					}
				}
			}
			else
			{
				myHTML.Append(hb).Append(EvoLang.NoGraph).Append(he);
			}
			myHTML.Append(EvoUI.clearer);
			return myHTML.ToString();
		}

#endregion

//### Forms Support HTML ############################################################################## 
#region "Forms Support HTML"

		private string chartTitle(string title)
		{
			return String.Format("<div class=\"chartTitle\">{0}</div>", title);
		}

		private string LastSearchCondition(bool canChooseAll)
		{
			String buffer;
			StringBuilder myHTML = new StringBuilder();

			myHTML.Append("<div class=\"FieldReadOnly\">");
			buffer = GetCacheKey(def_Data.dbtable);
			if (Page.Cache[buffer + "_W2"] != null)
				buffer = Page.Cache[buffer + "_W2"].ToString();
			else
				buffer = null;
			if (string.IsNullOrEmpty(buffer) || buffer.Equals(EvoLang.allEntities))
				myHTML.Append(EvoLang.allEntities);
			else if (canChooseAll)
			{
				String fieldName = selFieldID;
				myHTML.Append(EvoUI.HTMLInputRadio(fieldName, s1, EvoLang.allEntities, false, "evol_q1"))
					.Append(EvoUI.HTMLInputRadio(fieldName, s0, buffer, true, "evol_q0"));
			}
			else
			{ 
				myHTML.AppendFormat("<p>{0}</p>", buffer);
			}
			myHTML.Append("</div><br/>");
			return myHTML.ToString();
		}

		private string FormButtons(int eDisplayMode, bool editOK)
		{
			/// <summary>Buttons Save, Save and Add, Cancel / Edit (used on bottom of forms).</summary>
			StringBuilder myHTML = new StringBuilder();

			myHTML.Append("<div class=\"PanelButtons\">");
			if (eDisplayMode > 0) //1=edit, 2=new 
			{
				myHTML.Append("<input class=\"IndentBtn Button\" type=\"button\" name=\"save\" value=\" ").Append(EvoLang.Save).Append(" \" onclick=\"EvoVal.validForm(1)\">");
				if (_DBAllowInsert && _ItemID == 0)
				{
					myHTML.Append("&nbsp;<input type=\"button\" name=\"saveadd\" value=\" ").Append(EvoLang.SaveAdd).Append(" \" class=\"Button2\" onclick=\"EvoVal.validForm(9)\">");
				}
				myHTML.Append("&nbsp;<input type=\"button\" name=\"Cancel\" value=\" ")
					.Append((_ItemID == 0)?EvoLang.Cancel:EvoLang.View)
					.Append(" \" class=\"Button2\" onclick=\"EvPost('0')\">");
			}
			//view 
			else if (!_DBReadOnly && _DBAllowUpdate && editOK)
			{
				myHTML.Append("<input class=\"IndentBtn Button\" type=\"button\" name=\"Edit\" value=\" ").Append(EvoLang.Edit).Append(" \" onclick=\"EvPost('1')\">");
			}
			myHTML.Append("</div>");
			return myHTML.ToString();
		}

		private string HTMLNavLinks()
		{
			/// <summary>returns HTML for navigation links "first", "prev.", "next", "last", and "export".</summary>
			StringBuilder myHTML = new StringBuilder();

			bool YesNo = _NavLinks || _DBAllowExport;
			if (YesNo)
				myHTML.Append("<table class=\"Holder\"><tr class=\"RowFoot\"><td>");
			if (_NavLinks)
			{
				//first, prev., next, last navigation for records  
				bool nav_first, nav_last;
				string buffer = (_DisplayMode.Equals(1)) ? "30" : "20";
				myHTML.Append(EvoUI.HTMLSpace).Append(EvoUI.HTMLLinkEventRef("110", EvoLang.allEntities)).Append(": ");
				myHTML.Append("<span class=\"Paging\">");
				if (string.IsNullOrEmpty(navBar))
				{
					if (string.IsNullOrEmpty(def_Data.spget))
					{
						nav_first = nav != 1 & navd != 1;
						nav_last = nav != 4 & navd != 3;
					}
					else
					{
						nav_first = true;
						nav_last = true;
					}
				}
				else
				{
					nav_first = navBar.Substring(0, 1).Equals(s1);
					nav_last = EvoTC.Right(navBar, 1).Equals(s1);
				}
				if (nav_first)
				{
					myHTML.Append(EvoUI.HTMLLinkEventRef(buffer + s1, EvoLang.pFirst))
						.Append(EvoUI.HTMLLinkEventRef(buffer + s2, EvoLang.pPrev));
				}
				if (nav_last)
				{
					myHTML.Append(EvoUI.HTMLLinkEventRef(buffer + s3, EvoLang.pNext))
						.Append(EvoUI.HTMLLinkEventRef(buffer + s4, EvoLang.pLast));
				}
				myHTML.Append("</span>");
			}
			if (YesNo)
				myHTML.Append("</td><td align=\"right\">");
			if (_ItemID > 0 && _DBAllowExport)
				myHTML.Append(EvoUI.HTMLLinkEventRef("72", string.Format(EvoLang.ExportEntity, def_Data.entity)));
			if (YesNo)
				myHTML.Append("&nbsp;</td></tr></table>");
			return myHTML.ToString();
		}

		private string HTMLTabList(XmlNodeList nodeListTabs, int tabActiveIdx, int nbTabs, EvolTabPosition tabPosition)
		{
			/// <summary>returns HTML for all tabs for the form.</summary>
			StringBuilder myHTML = new StringBuilder();
			bool TabLeft = _TabPosition != EvolTabPosition.Top;
			bool notIE = !IEbrowser;

			if (notIE)
				myHTML.Append("<div style=\"height:10px\"></div>");
			if (TabLeft)
				myHTML.Append("<table width=\"100%\"><tr valign=top><td style=\"width:120px\">");
			myHTML.Append("<div class=\"TabBar\"><div class=\"TabHolder");
			if (TabLeft)
				myHTML.Append(" TabLeft");
			myHTML.Append("\" id=\"evoTabs\">");
			if (TabLeft && notIE)
				for (int i = 0; i < nbTabs; i++)
					myHTML.AppendFormat("<p>{0}</p>", EvoUI.HTMLbuttonTab(nodeListTabs[i].Attributes[xAttribute.label].Value, i == tabActiveIdx));
			else
				for (int i = 0; i < nbTabs; i++)
					myHTML.Append(EvoUI.HTMLbuttonTab(nodeListTabs[i].Attributes[xAttribute.label].Value, i == tabActiveIdx));
			myHTML.Append("</div></div>");
			if (TabLeft)
				myHTML.Append("</td><td>");
			myHTML.Append(EvoUI.HTMLInputHidden("EvoActTab", tabActiveIdx.ToString()));
			if (notIE)
				myHTML.Append("<div style=\"height:5px\"></div>");
			return myHTML.ToString();
		}
		
		private string HTMLPanelFields(XmlNode aPanelNode, int gDisplayMode, string panelID, bool withData, int PanelPosition)
		{
			/// <summary>build the html for 1 panel (in 'edit' or 'view' modes) .</summary>
			/// <remarks>this is used in View and Edit forms.</remarks>  
			string sql, stylePanel, buffer, buffer1, buffer2, target;
			string dbcolumnAttribute = xAttribute.dbColumn;
			int fWidth = 0, fHeight = 0, fMaxLength;
			string fieldValue = null;
			string fieldFormat = null;
			int fieldReadOnly;
			string fieldLabel = null, fieldLabel2 = null;
			int nbFileUploads = 0;
			int fid;
			int i, maxLoopXML;
			StringBuilder myResult = new StringBuilder();
			StringBuilder htmlField;
			StringBuilder hiddenFields = new StringBuilder();

			if (aPanelNode.Attributes[xAttribute.cssClass] == null)
				stylePanel = EvoUI.cssPanel;
			else
			{
				stylePanel = aPanelNode.Attributes[xAttribute.cssClass].Value;
				if (string.IsNullOrEmpty(stylePanel))
					stylePanel = EvoUI.cssPanel;
			}
			myResult.AppendFormat("<div class=\"{0}\">",stylePanel);
			string fieldName = aPanelNode.Attributes[xAttribute.label].Value;
			if (_ShowDesigner)
				fieldName += EvoUI.LinkDesigner(EvoUI.DesType.pnl, _FormID, fieldName, _PathDesign);
			if (aPanelNode.Attributes[xAttribute.cssClassLabel] == null)
				stylePanel = EvoUI.cssPanelLabel;
			else
			{
				stylePanel = aPanelNode.Attributes[xAttribute.cssClassLabel].Value;
				if (string.IsNullOrEmpty(stylePanel))
					stylePanel = EvoUI.cssPanelLabel;
			}
			myResult.Append(EvoUI.HTMLPanelLabel(fieldName, "evoPnl" + panelID, stylePanel, _CollapsiblePanels, IEbrowser));
			bool inTable = false;
			int linewidth = 0;
			myResult.AppendFormat("<span ID=\"evoPnl{0}\">", panelID);
			maxLoopXML = aPanelNode.ChildNodes.Count;
			//view 
			if (gDisplayMode == 0)
			{
				dbcolumnAttribute += "read";
				fieldReadOnly = 1;
			}
			else
				fieldReadOnly = 0;
			DataRow r0 = null;
			if (withData && ds.Tables[0].Rows.Count > 0)
				r0 = ds.Tables[0].Rows[0];
			//for each FIELD 
			for (i = 0; i < maxLoopXML; i++)
			{
				XmlNode cn = aPanelNode.ChildNodes[i];
				if (cn.NodeType == XmlNodeType.Element)
				{
					htmlField = new StringBuilder();
					string fType = cn.Attributes[xAttribute.type].Value;
					sql = cn.Attributes[dbcolumnAttribute].Value;
					fieldName = UID + sql;
					if (fType != "hidden")
					{
						fWidth = Convert.ToInt32(cn.Attributes[xAttribute.width].Value);
						htmlField.AppendFormat("<td width=\"{0}%\">", fWidth);
						fieldLabel = cn.Attributes[xAttribute.label].Value;
						//##### - edit mode 
						if (gDisplayMode > 0)
						{
							if (fType.Equals(EvoDB.t_formula))
								fieldReadOnly = 1;
							else
							{
								if (cn.Attributes[xAttribute.dbReadOnly] == null)
									fieldReadOnly = 0;
								else if (EvoTC.isInteger(cn.Attributes[xAttribute.dbReadOnly].Value))
								{
									fieldReadOnly = Convert.ToInt32(cn.Attributes[xAttribute.dbReadOnly].Value);
									//INSERT 
									if (_ItemID < 1 && fieldReadOnly == 2)
										fieldReadOnly = 0;
								}
							}
							if (string.IsNullOrEmpty(fieldLabel) && (cn.Attributes[xAttribute.labelEdit] != null))
								fieldLabel = cn.Attributes[xAttribute.labelEdit].Value;
							fieldLabel2 = fieldLabel;
							if (fieldReadOnly.Equals(0) && (cn.Attributes[xAttribute.required] != null))
							{
								if (cn.Attributes[xAttribute.required].Value == s1)
									fieldLabel += EvoUI.HTMLFlagRequired;
							}
							if (fType.Equals(EvoDB.t_lov))
							{
								if (fieldReadOnly > 0)
								{
									if (cn.Attributes[xAttribute.dbColumnRead] == null)
										sql = cn.Attributes[xAttribute.dbColumn].Value;
									else
										sql = cn.Attributes[xAttribute.dbColumnRead].Value;
								}
								else
									sql = cn.Attributes[xAttribute.dbColumn].Value;
							}
							else if (fType.Equals(EvoDB.t_formula))
								sql = cn.Attributes[xAttribute.dbColumnRead].Value;
							else
								sql = cn.Attributes[dbcolumnAttribute].Value;
						}
						//##### - all modes 
						if (_ShowDesigner)
						{
							fid = Convert.ToInt32(cn.Attributes[xAttribute.id].Value);
							fieldLabel += EvoUI.LinkDesigner(EvoUI.DesType.fld, fid, fieldLabel2, _PathDesign);
						}
						if (withData && ds.Tables[0].Rows.Count > 0)
						{
							if (!string.IsNullOrEmpty(sql))
							{
								try
								{
									if (object.ReferenceEquals(r0[sql], DBNull.Value))
										fieldValue = string.Empty;
									else
										fieldValue = r0[sql].ToString();
								}
								catch (Exception ex)
								{
									fieldValue = string.Empty;
									AddError(ex.Message);
								}
							}
							else
							{
								if (fType.Equals(xAttribute.url) && (cn.Attributes[xAttribute.link] != null))
								{
									fieldValue = cn.Attributes[xAttribute.link].Value;
									if (string.IsNullOrEmpty(sql))
										sql = " ";
								}
								else
									fieldValue = string.Empty;
							}
						}
						else
							fieldValue = string.Empty;
						bool fieldMustShow = true;
						if (cn.Attributes[xAttribute.optional] != null && string.IsNullOrEmpty(fieldValue) && gDisplayMode == 0 && fType != EvoDB.t_lov)
						{
							fieldMustShow = !cn.Attributes[xAttribute.optional].Value.Equals(s1);
						}
						if ((fieldMustShow || gDisplayMode == 1) && fieldLabel.Length > 0)
						{
							if (cn.Attributes[xAttribute.cssClassLabel] != null)
								htmlField.AppendFormat("<span class=\"{0}\"", cn.Attributes[xAttribute.cssClassLabel].Value);
							else
							{
								htmlField.Append("<span class=\"FieldLabel");
								if(EvoLang.R2L) 
									htmlField.Append("R2L"); // "FieldLabelR2L" not "FieldLabel R2L" more than rtl
								htmlField.Append("\"");
							}
							if (fieldReadOnly == 0 && fType == EvoDB.t_txtm && !_ShowDesigner)
								htmlField.Append(" onmouseover=\"javascript:EvoUI.showResize('").Append(fieldName).Append("',-1,this)\">");
							else
								htmlField.Append(">");
							htmlField.Append(fieldLabel);
							htmlField.Append("</span><br/>");
						}
						string fieldCSS = (fieldReadOnly > 0) ? xAttribute.cssClassView : xAttribute.cssClass;
						if (cn.Attributes[fieldCSS] == null)
							fieldCSS = null;
						else
							fieldCSS = cn.Attributes[fieldCSS].Value;
						if (fieldReadOnly > 0)
						{
							//##### - VIEW mode ######################################################################
							if (fieldMustShow)
							{
								if (!fType.Equals(EvoDB.t_pix))
								{
									if (string.IsNullOrEmpty(fieldCSS))
										htmlField.Append("<div class=\"FieldReadOnly\">");
									else
										htmlField.AppendFormat("<div class=\"{0}\">", fieldCSS);
								}
								if (!string.IsNullOrEmpty(sql))
								{
									switch (fType)
									{
										case EvoDB.t_text:
											if (PanelPosition == 0 && i == 0 && icon != string.Empty)
												htmlField.Append(icon);
											htmlField.Append(EvoTC.Text2HTMLwBR(fieldValue));
											break;
										case EvoDB.t_lov:
											buffer1 = string.Empty;
											buffer2 = string.Empty;
											if (string.IsNullOrEmpty(def_Data.dblockingcolumn))
											{
												if (cn.Attributes[xAttribute.lovMany] != null)
													buffer2 = cn.Attributes[xAttribute.lovMany].Value;
											}
											else if (withData && def_Data.dblockingcolumn == cn.Attributes[xAttribute.dbColumn].Value)
											{
												dbwherelock = String.Format("{0}={1}", def_Data.dblockingcolumn, r0[def_Data.dblockingcolumn].ToString());
												// dbLockinglabel = "????" 
												Page.Cache["locking"] = dbwherelock;
											}
											if (buffer2 != string.Empty)
											{
												if (cn.Attributes[xAttribute.link] == null)
													buffer2 = string.Empty;
												else
													buffer2 = cn.Attributes[xAttribute.link].Value;
												htmlField.Append(HTMLlovMany(cn, 0, buffer2));
											}
											else
											{
												if (!string.IsNullOrEmpty(fieldValue))
												{
													if (cn.Attributes[xAttribute.dbColumnIcon] == null)
														buffer1 = string.Empty;
													else
													{
														buffer1 = cn.Attributes[xAttribute.dbColumnIcon].Value;
														try
														{
															if (!string.IsNullOrEmpty(buffer1))
																buffer1 = r0[buffer1].ToString();
														}
														catch
														{
															buffer1 = string.Empty;
														}
													}
													if (buffer1.Length > 0)
														buffer1 = EvoUI.HTMLImg(_PathPix + buffer1) + EvoUI.HTMLSpace;
													if (cn.Attributes[xAttribute.link] == null)
														fieldFormat = string.Empty;
													else
														fieldFormat = cn.Attributes[xAttribute.link].Value;
													fieldValue = EvoTC.Text2HTML(fieldValue);
													if (fieldFormat.Length > 0)
													{
														if (cn.Attributes[xAttribute.linkTarget] == null)
															target = string.Empty;
														else
															target = cn.Attributes[xAttribute.linkTarget].Value;
														fieldFormat = fieldFormat.Replace(EvoDB.p_itemid, r0[cn.Attributes[xAttribute.dbColumn].Value].ToString());
														if (cn.Attributes[xAttribute.linkLabel] == null)
															buffer2 = fieldValue;
														else
														{
															buffer2 = cn.Attributes[xAttribute.linkLabel].Value;
															buffer2 = EvoTC.Text2HTML(buffer2.Replace("@fieldvalue", fieldValue));
														}
														fieldValue = EvoUI.HTMLLink(fieldFormat, buffer2, target, null);
													}
													htmlField.Append(buffer1).Append(fieldValue);
												}
											}
											break;
										case EvoDB.t_int:
										case EvoDB.t_dec:
										case EvoDB.t_formula:
											if (cn.Attributes[xAttribute.format] == null)
												fieldFormat = string.Empty;
											else
												fieldFormat = cn.Attributes[xAttribute.format].Value;
											if (string.IsNullOrEmpty(fieldFormat))
												htmlField.Append(fieldValue);
											else if (fieldValue != String.Empty)
											{
												try
												{
													if (fType == EvoDB.t_int)
														buffer2 = Convert.ToInt32(fieldValue).ToString(fieldFormat);
													else
														buffer2 = EvoTC.String2Dec(fieldValue).ToString(fieldFormat);
												}
												catch
												{
													buffer2 = fieldValue;
												}
												if (!String.IsNullOrEmpty(buffer2))
													htmlField.Append(EvoUI.noBR(buffer2));
											}
											break;
										case EvoDB.t_txtm:
										case EvoDB.t_html: // same as t_txtm, but no encoding here b/c HTML is HTML 
											if (cn.Attributes[xAttribute.height] == null)
												fHeight = 3;
											else
												fHeight = EvoTC.String2Int(cn.Attributes[xAttribute.height].Value);
											htmlField.Append("<span style=\"height:").Append(fHeight * 20).Append("\">");
											if (!string.IsNullOrEmpty(fieldValue))
											{
												// encode textmultiline but not html
												// warning: using html fields may expose to cross-site scripting
												if (fType == EvoDB.t_txtm)
													htmlField.Append(EvoTC.Text2HTMLwBR(fieldValue));
												else
													htmlField.Append(fieldValue); 
											}
											htmlField.Append("<br/></span>");
											break;
										case EvoDB.t_bool:
											if (fieldValue.Equals(s1) || fieldValue.Equals(sTrue))
											{
												htmlField.Append(EvoUI.HTMLSpace);
												if (cn.Attributes[xAttribute.img] == null)
													htmlField.Append(EvoUI.HTMLImgCheckMark(IEbrowser, _PathPixToolbar));
												else
												{
													fieldFormat = cn.Attributes[xAttribute.img].Value;
													if (string.IsNullOrEmpty(fieldFormat))
														htmlField.Append(EvoUI.HTMLImgCheckMark(IEbrowser, _PathPixToolbar));
													else
														htmlField.Append(EvoUI.HTMLImg(_PathPixToolbar + fieldFormat, EvoLang.Checked));
												}
											}
											break;
										case EvoDB.t_date:
										case EvoDB.t_datetime:
										case EvoDB.t_time:
											if (EvoTC.isDate(fieldValue))
											{
												if (cn.Attributes[xAttribute.format] == null)
													fieldFormat = string.Empty;
												else
													fieldFormat = cn.Attributes[xAttribute.format].Value;
												if (string.IsNullOrEmpty(fieldFormat))
													fieldFormat = EvoTC.DefaultDateFormat(fType);
												else
												{
													fieldFormat = "{0:" + fieldFormat + "}";
												}
												htmlField.Append(EvoTC.HTMLDateFormated(fType, fieldValue, fieldFormat));
											}
											break;
										case EvoDB.t_email:
											if (fieldValue != string.Empty)
												htmlField.Append(EvoUI.HTMLEmail(fieldValue));
											break;
										case EvoDB.t_url:
											if (fieldValue != string.Empty)
											{
												if (cn.Attributes[xAttribute.linkLabel] == null)
													fieldFormat = string.Empty;
												else
													fieldFormat = cn.Attributes[xAttribute.linkLabel].Value.Trim();
												if (sql == " ")
												{
													if (fieldValue.IndexOf("@item") > -1)
														fieldValue = fieldValue.Replace(EvoDB.p_itemid, _ItemID.ToString());
													if (string.IsNullOrEmpty(fieldFormat))
														fieldFormat = fieldValue;
													fieldValue = EvoUI.HTMLLink(HttpUtility.HtmlEncode(fieldValue), fieldFormat);
												}
												else
												{
													if (fieldFormat.Length > 0)
														fieldValue = EvoUI.HTMLLink(HttpUtility.HtmlEncode(fieldValue), fieldFormat, inNewBrowser, null);
													else
													{
														if (cn.Attributes[xAttribute.img] == null)
															fieldFormat = string.Empty;
														else
															fieldFormat = cn.Attributes[xAttribute.img].Value.Trim();
														if (fieldFormat.Length == 0)
														{
															try
															{
																fieldFormat = cn.Attributes[xAttribute.dbColumnIcon].Value.Trim();
																if (fieldFormat != string.Empty)
																	fieldFormat = Convert.ToString(r0[fieldFormat]);
															}
															catch
															{
																fieldFormat = string.Empty;
															}
														}
														fieldValue = HttpUtility.HtmlEncode(fieldValue);
														if (fieldFormat != string.Empty)
															fieldValue = EvoUI.HTMLLink(fieldValue, string.Empty, inNewBrowser, _PathPix + fieldFormat);
														else
															fieldValue = EvoUI.HTMLLink(fieldValue, fieldValue, inNewBrowser, null);
													}
												}
												htmlField.Append(fieldValue);
											}
											break;
										case EvoDB.t_pix:
											if (string.IsNullOrEmpty(fieldValue))
												htmlField.Append(EvoLang.NA);
											else
											{
												if (cn.Attributes[xAttribute.link] == null)
													fieldFormat = string.Empty;
												else
												{
													fieldFormat = cn.Attributes[xAttribute.link].Value;
													if (fieldFormat != string.Empty)
													{
														fieldFormat = fieldFormat.Replace(EvoDB.p_itemid, r0["ID"].ToString());
														htmlField.Append("<a href=\"").Append(fieldFormat);
														if (cn.Attributes[xAttribute.linkTarget] != null)
														{
															target = cn.Attributes[xAttribute.linkTarget].Value;
															if (target.Length > 0)
																htmlField.Append("\" target=\"").Append(target);
														}
														htmlField.Append("\">");
													}
												}
												if (cn.Attributes[xAttribute.cssClass] != null)
													buffer = cn.Attributes[xAttribute.cssClass].Value;
												else
													buffer = "fieldImg";
												htmlField.AppendFormat("<img src=\"{0}{1}\" class=\"{2} Icon\" alt=\"\"/>", _PathPix, fieldValue, buffer);
												if (fieldFormat != string.Empty)
													htmlField.Append("</a>");
											}
											break;
										case EvoDB.t_doc:
											if (string.IsNullOrEmpty(fieldValue))
												htmlField.Append(EvoLang.NA);
											else
											{
												if (cn.Attributes[xAttribute.dbColumnIcon] == null)
													fieldFormat = string.Empty;
												else
												{
													fieldFormat = cn.Attributes[xAttribute.dbColumnIcon].Value.Trim();
													try
													{
														if (!string.IsNullOrEmpty(fieldFormat))
															fieldFormat = Convert.ToString(r0[fieldFormat]);
													}
													catch
													{
														fieldFormat = string.Empty;
													}
													if (!string.IsNullOrEmpty(fieldFormat))
														htmlField.Append(EvoUI.HTMLLink(_PathPix + fieldValue, String.Empty, inNewBrowser, _PathPix + fieldFormat)).Append(EvoUI.tag_BR);
												}
												htmlField.Append(EvoUI.HTMLLink(_PathPix + fieldValue, HttpUtility.HtmlEncode(fieldValue), inNewBrowser, null));
											}
											break;
										default:
											htmlField.Append(HttpUtility.HtmlEncode(fieldValue));
											break;
									}
								}
								if (!fType.Equals(EvoDB.t_pix))
									htmlField.Append("&nbsp;</div>");
							}
						}
						else
						{
							//##### - EDIT mode ###################################################################### 
							fMaxLength = xAttribute.GetFieldMaxLength(cn);
							if (string.IsNullOrEmpty(fieldCSS))
								fieldCSS = "Field";
							switch (fType)
							{
								case EvoDB.t_text:
								case EvoDB.t_email:
								case EvoDB.t_url:
								case EvoDB.t_int:
								case EvoDB.t_dec:
									fieldValue = HttpUtility.HtmlEncode(fieldValue);
									htmlField.Append("<input type=\"text\"").AppendFormat(EvoUI.HTMLNameIdClass, fieldName, fieldName, fieldCSS);
									if (fMaxLength > 0)
										htmlField.AppendFormat(" maxlength=\"{0}\"", fMaxLength);
									if (fType.Equals(EvoDB.t_int) || fType.Equals(EvoDB.t_dec))
										htmlField.Append(" OnKeyUp=\"EvoVal.checkNum(this,'").Append(fType.Substring(0, 1)).Append("')\"");
									htmlField.AppendFormat(" value=\"{0}\">", fieldValue);
									break;
								case EvoDB.t_txtm:
								case EvoDB.t_html: 
									fieldValue = HttpUtility.HtmlEncode(fieldValue);
									if (cn.Attributes[xAttribute.height] == null)
										fHeight = 3;
									else
									{
										fHeight = EvoTC.String2Int(cn.Attributes[xAttribute.height].Value);
										if (fHeight < 1)
											fHeight = 1;
									}
									htmlField.AppendFormat("<textarea class=\"{0}\" style=\"height:{1};width:100%\" rows=\"{2}", fieldCSS, fHeight * 20, fHeight);
									if (fType == EvoDB.t_txtm && fMaxLength > 0)
									{
										htmlField.Append("\" cols=\"52\" onKeyUp=\"EvoVal.checkMaxLen(this,").Append(fMaxLength).Append(")");
									} 
									htmlField.AppendFormat("\" name=\"{0}\" id=\"{0}\">", fieldName)
										.Append(fieldValue).Append("</textarea>");
									break;
								case EvoDB.t_lov:
									// edit lookup for lov 
									if (cn.Attributes["lookup"] == null)
										buffer = string.Empty;
									else
										buffer = cn.Attributes["lookup"].Value;
									if (buffer.Length > 0)
									{
										fieldValue = Page.Request[fieldName];
										if (EvoTC.String2Int(fieldValue) > 0)
										{
											lockDbname = buffer;
											loclValue = fieldName;
											//If dbLockingColumn <> String.Empty && (UID + sql == dbLockingColumn Then 	                                        
											//End If 
											htmlField.Append(EvoUI.HTMLInputHidden(fieldName, HttpUtility.HtmlEncode(fieldValue)));
											htmlField.Append("<div class=\"FieldReadOnly\">").Append(HTMLlov(cn, String.Empty, fieldValue, LOVFormat.HTML, Convert.ToInt32(fieldValue))).Append("</div>");
										}
										else
											buffer = string.Empty;
									}
									//buffer = String.Empty 
									//Try 
									// buffer = .Attributes(Attr.dbcolumnreadlov).Value 
									//Catch 
									//End Try 
									//buffer = LOVfromCache(EvoDB.t_lov & dbtable & buffer, "5") 
									//htmlField.Append(buffer) 
									// cache(key = LCase(EvoDB.t_lov & dbtable & A(Attr.dbtablelov) & (Attr.dbcolumnreadlov) & (Attr.dbColumnImg))) 
									else
									{
										htmlField.AppendFormat("<select name=\"{0}\" id=\"{0}\" class=\"{1}\">", fieldName, fieldCSS);
										if (withData)
										{
											if (cn.Attributes[xAttribute.lovMany] == null)
												buffer1 = string.Empty;
											else
												buffer1 = cn.Attributes[xAttribute.lovMany].Value;
											if (string.IsNullOrEmpty(buffer1))
											{
												htmlField.Append(EvoUI.HTMLOptionZero)
													.Append(HTMLlov(cn, String.Empty, fieldValue, LOVFormat.HTML, 0));
											}
											else
												htmlField.Append(HTMLlovMany(cn, Convert.ToInt32(r0[cn.Attributes[xAttribute.dbColumn].Value]), fieldFormat));
										}
										else
										{
											htmlField.Append(EvoUI.HTMLOptionZero);
											buffer = s0;
											if (_ItemID < 1 && (cn.Attributes["defaultvalue"] != null))
												buffer = EvoTC.String2Int(cn.Attributes["defaultvalue"].Value).ToString();
											htmlField.Append(HTMLlov(cn, String.Empty, buffer, LOVFormat.HTML, 0));
										}
										htmlField.Append("</select>");
									}
									break;
								//edit LOV 
								//maybe link to other window, maybe javascript only one item? 
								// htmlField.Append(EvoUI.HTMLLinkEventRef("1212", "<small>[Edit]</small>")) 
								case EvoDB.t_bool:
									htmlField.Append(EvoUI.HTMLInputCheckBox(fieldName, s1, fieldValue.Equals(sTrue) || fieldValue.Equals(s1)));
									break;
								case EvoDB.t_date:
								case EvoDB.t_datetime:
								case EvoDB.t_time:
									if (EvoTC.isDate(fieldValue))
									{
										//if (cn.Attributes[xAttribute.format] == null)
										//    fieldFormat = string.Empty;
										//else
										//    fieldFormat = cn.Attributes[xAttribute.format].Value;
										//if (string.IsNullOrEmpty(fieldFormat))
											fieldFormat = EvoTC.DefaultDateFormat(fType);
										fieldValue = EvoTC.HTMLDateFormated(fType, fieldValue, fieldFormat);
									}
									htmlField.Append(EvoUI.HTMLInputDate(fieldName, fieldValue, _Language, _PathPixToolbar));
									break;
								case EvoDB.t_pix:
								case EvoDB.t_doc:
									htmlField.Append("<div class=\"smallTxt\">");
									if (fieldValue != string.Empty)
										htmlField.Append("<span class=\"FieldReadOnly\">").Append(fieldValue).Append("</span><br/>");
									if (fType.Equals(EvoDB.t_pix))
									{
										htmlField.Append("<br/><img src=\"");
										if (string.IsNullOrEmpty(fieldValue))
											htmlField.Append(_PathPixToolbar).Append("imgno.gif\" ID=\"");
										else
											htmlField.Append(_PathPix).Append(fieldValue).Append("\" ID=\"");
										htmlField.Append(fieldName).Append("img\" alt=\"\" class=\"FieldImg\"/><br/>");
									}
									buffer = string.Format("UP-evol{0}", i);
									htmlField.Append(EvoUI.HTMLInputHidden(fieldName + "_dp", string.Empty));
									if (IEbrowser)
										htmlField.Append(EvoUI.HTMLLinkShowVanish(buffer, EvoLang.NewUpload));
									if (fieldValue != string.Empty)
									{
										htmlField.Append("<br/>&nbsp;<a href=\"Javascript:Evol.");
										string pJS = (fType.Equals(EvoDB.t_pix)) ? "pixM" : "docM"; 
										htmlField.AppendFormat("{0}('{1}')\">{2}</a>", pJS, fieldName, EvoLang.Delete);
									}
									htmlField.Append("</div><br/>").Append(EvoUI.HTMLDiv(buffer, !IEbrowser));
									htmlField.Append("<input type=\"file\" class=\"Field\" name=\"").AppendFormat("{0}\" id=\"{0}", fieldName)
										.Append("\" value=\"").Append(HttpUtility.HtmlEncode(fieldValue)).Append("\" onchange=\"e$('").Append(fieldName);
									if (fType.Equals(EvoDB.t_pix))
										htmlField.Append("img').src='").Append(_PathPixToolbar).Append("imgupdate.gif';e$('").Append(fieldName);
									htmlField.Append("_dp').value=''\"><br/></div>");
									nbFileUploads += 1;
									break;
								default:
									htmlField.Append("<input type=\"text\" class=\"Field\" name=\"").AppendFormat("{0}\" id=\"{0}\" value=\"{1}\">", fieldName, HttpUtility.HtmlEncode(fieldValue));
									break;
							}
							if (withData && !string.IsNullOrEmpty(fieldValue))
								htmlField.Append(EvoUI.HTMLInputHidden(String.Format( "{0}_ov", fieldName), HttpUtility.HtmlEncode(fieldValue)));
						}
						htmlField.Append("</td>");
					}
					else
						hiddenFields.Append(EvoUI.HTMLInputHidden(fieldName, fieldValue));
					int fw = linewidth + fWidth;
					switch (fw)
					{
						case 100:
							if (fWidth < 100)
							{
								myResult.Append(htmlField).Append(trTableEnd);
								inTable = false;
							}
							else
								myResult.Append(tableBeginTr2).Append(htmlField).Append(trTableEnd);
							linewidth = 0;
							break;
						default:
							if (fw > 100)
							{
								if (inTable)
									myResult.Append(trTableEnd);
								myResult.Append(tableBeginTr2).Append(htmlField);
								inTable = true;
								linewidth = fWidth;
							}
							else
							{
								if (linewidth == 0)
								{
									if (inTable)
										myResult.Append(trTableEnd);
									myResult.Append(tableBeginTr2);
									inTable = true;
								}
								myResult.Append(htmlField);
								linewidth += fWidth;
							}
							break;
					}
				}
			}
			if (i > 0 && inTable)
				myResult.Append(trTableEnd);
			myResult.Append("</span>");
			if (hiddenFields.Length > 0)
				myResult.Append(hiddenFields);
			myResult.Append("</div>");
			return myResult.ToString();
		}
	
		//private string HTMLAlphabetLinks()
		//{
		//    StringBuilder myHTML = new StringBuilder();
		//    string buffer = null;
		//    //not always "evo1" 
		//    {
		//        myHTML.Append("<a href=\"javascript:__doPostBack('evo1','q:~A')\">A");
		//        for (int i = 66; i <= 90; i++)
		//        {
		//            myHTML.Append("</a> - <a href=\"javascript:__doPostBack('evo1','q:~").Append(Strings.Chr(i)).Append("')\">").Append(Strings.Chr(i));
		//        }
		//        myHTML.Append("</a> - <a href=").Append("javascript:__doPostBack('evo1','q:~0')>#");
		//    }
		//    return myHTML.ToString;
		//} 

#endregion 
	
	}
}

