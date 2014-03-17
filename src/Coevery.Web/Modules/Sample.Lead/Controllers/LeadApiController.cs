﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Sample.Lead.Models;

namespace Sample.Lead.Controllers
{
    public class LeadApiController : ApiController {
        public IEnumerable<LeadPartRecord> GetAllLeads() {
            var leads = new List<LeadPartRecord> {
                new LeadPartRecord {Id = 1, Subject = "Long-term Sitefinity partner", Description = "", CompanyName = ""},
                new LeadPartRecord {Id = 2, Subject = "Web Part Needed", Description = "I need a w", CompanyName = ""},
                new LeadPartRecord {Id = 3, Subject = "DNN convert", Description = "", CompanyName = ""},
                new LeadPartRecord {Id = 4, Subject = "Magento Specialist", Description = "", CompanyName = "DiBowebsites"},
                new LeadPartRecord {Id = 5, Subject = "Significant Amount of Custom DNN Module Creation ", Description = "Message: ", CompanyName = ""},
                new LeadPartRecord {Id = 6, Subject = "Costs to copy the air b'n'b site ", Description = "", CompanyName = ""},
                new LeadPartRecord {Id = 7, Subject = "build a bilingual (Chinese & English) website", Description = "", CompanyName = ""},
                new LeadPartRecord {Id = 8, Subject = "need to offshore some small projects", Description = "客户说有很多小项目需", CompanyName = ""},
                new LeadPartRecord {Id = 9, Subject = "Fixing the issues for New Site", Description = "Message: ", CompanyName = "BagsandFashion"},
                new LeadPartRecord {Id = 10, Subject = "Senior developer in advanced C#", Description = "", CompanyName = "matogen.com"},
                new LeadPartRecord {Id = 11, Subject = "Nop Theme", Description = "", CompanyName = ""},
                new LeadPartRecord {Id = 12, Subject = "Job posting website", Description = "I need .ne", CompanyName = ""},
                new LeadPartRecord {Id = 13, Subject = "Quotation for a control library", Description = "", CompanyName = "西门子"},
                new LeadPartRecord {Id = 14, Subject = "Two Full Time Silverlight Engineers ", Description = "I have see", CompanyName = ""},
                new LeadPartRecord {Id = 15, Subject = "Calendar Carousel on Drupal Website", Description = "", CompanyName = "webconcept.de"},
                new LeadPartRecord {Id = 16, Subject = "new payment gateway implementation", Description = "客户的需求是实现一个", CompanyName = "agent database"},
                new LeadPartRecord {Id = 17, Subject = "Orchard CMS Maintanece", Description = "Message: ", CompanyName = ""},
                new LeadPartRecord {Id = 18, Subject = "nopcommerce template", Description = "I would li", CompanyName = ""},
                new LeadPartRecord {Id = 19, Subject = "Technical Partner to Outsource", Description = "Message: ", CompanyName = ""},
                new LeadPartRecord {Id = 20, Subject = "a simple nopCommerce plugin", Description = "", CompanyName = ""},
                new LeadPartRecord {Id = 21, Subject = "get Nopcommerce 2.40 to play uploaded videos", Description = "留言I need h", CompanyName = "http://www.mywaycigs.com/"},
                new LeadPartRecord {Id = 22, Subject = "Mobile Project Development", Description = "Skype上找到我们", CompanyName = ""},
                new LeadPartRecord {Id = 23, Subject = "Sedunia Payment Management", Description = "", CompanyName = "GoQuo"},
                new LeadPartRecord {Id = 24, Subject = "duplicating registration page on DNN website", Description = "", CompanyName = ""},
                new LeadPartRecord {Id = 25, Subject = "Joomla membership site ", Description = "", CompanyName = ""},
                new LeadPartRecord {Id = 26, Subject = "Building a websited with Sitefinity", Description = "Nick是一个公司的", CompanyName = ""},
                new LeadPartRecord {Id = 27, Subject = "Nopcommerce intergration with xero", Description = "", CompanyName = "Akrom"},
                new LeadPartRecord {Id = 28, Subject = "PSD conversion into Sitefinity", Description = "Message: ", CompanyName = ""},
                new LeadPartRecord {Id = 29, Subject = "Changes to Prestashop template", Description = "", CompanyName = "ellelocks"},
                new LeadPartRecord {Id = 30, Subject = "looking to outsource various projects ", Description = "", CompanyName = "Kitesystems"},
                new LeadPartRecord {Id = 31, Subject = "WPAchievements plugin", Description = "", CompanyName = ""},
                new LeadPartRecord {Id = 32, Subject = "Offshore Partnership", Description = "", CompanyName = ""},
                new LeadPartRecord {Id = 33, Subject = "DNN Developer", Description = "", CompanyName = ""},
                new LeadPartRecord {Id = 34, Subject = "dedicated programer from China", Description = "", CompanyName = "BBsecurities"},
                new LeadPartRecord {Id = 35, Subject = "Implementing skins for a GUI & Web designer ", Description = "", CompanyName = ""},
                new LeadPartRecord {Id = 36, Subject = "网站改版升级项目", Description = "", CompanyName = ""},
                new LeadPartRecord {Id = 37, Subject = "FME Would-be Partner", Description = "", CompanyName = ""},
                new LeadPartRecord {Id = 38, Subject = "Good Developer on Long Term Basis", Description = "德国客户", CompanyName = "www.server-ware.com"},
                new LeadPartRecord {Id = 39, Subject = "试用CRM", Description = "", CompanyName = ""},
                new LeadPartRecord {Id = 40, Subject = "汽车养护公司管理系统", Description = "客户是做汽车行业的，", CompanyName = "国宾汽车养护"},
                new LeadPartRecord {Id = 41, Subject = "WP Error", Description = "", CompanyName = ""},
                new LeadPartRecord {Id = 42, Subject = "developing NOP commerce plugins", Description = "", CompanyName = "luminet.si"},
                new LeadPartRecord {Id = 43, Subject = "Assistance with nopcommerce development", Description = "", CompanyName = ""},
                new LeadPartRecord {Id = 44, Subject = "A solution for sharing and saving documents to client files", Description = "", CompanyName = ""},
                new LeadPartRecord {Id = 45, Subject = "Building a website for a ski resort", Description = "判断潜力小且一直没有", CompanyName = ""},
                new LeadPartRecord {Id = 46, Subject = "website development services ", Description = "", CompanyName = ""},
                new LeadPartRecord {Id = 47, Subject = "Customized Mega Menu for Your Website", Description = "Message: ", CompanyName = "Network Fiscale"},
                new LeadPartRecord {Id = 48, Subject = "百度轻应用", Description = "", CompanyName = ""},
                new LeadPartRecord {Id = 49, Subject = "Wordpress plugins for responsive web sites", Description = "", CompanyName = ""},
                new LeadPartRecord {Id = 50, Subject = "custom nopCommerce skins", Description = "", CompanyName = ""},
                new LeadPartRecord {Id = 51, Subject = "Annual Costs for 10 devs/testers", Description = "Message: ", CompanyName = ""},
                new LeadPartRecord {Id = 52, Subject = "Web application of tracking quotations", Description = "", CompanyName = "chobble.com"},
                new LeadPartRecord {Id = 53, Subject = "Customization to the blog feature", Description = "", CompanyName = "sitesee.net"},
                new LeadPartRecord {Id = 54, Subject = "C++ developer for image segmentation project", Description = "Charles Li", CompanyName = "Poikos"},
                new LeadPartRecord {Id = 55, Subject = "Cloud Storage System", Description = "Message: ", CompanyName = ""},
                new LeadPartRecord {Id = 56, Subject = "New Shopify Store", Description = "", CompanyName = ""},
                new LeadPartRecord {Id = 57, Subject = "UI piece", Description = "", CompanyName = ""},
                new LeadPartRecord {Id = 58, Subject = "Web site with PHP scripts", Description = "", CompanyName = "Telematcia"},
                new LeadPartRecord {Id = 59, Subject = "Hotel Booking Site with NopCommerce", Description = "Message: ", CompanyName = ""},
                new LeadPartRecord {Id = 60, Subject = "support Prestashop developer", Description = "", CompanyName = ""},
                new LeadPartRecord {Id = 61, Subject = "Upgrading Magento website", Description = "", CompanyName = ""},
                new LeadPartRecord {Id = 62, Subject = "China Pay", Description = "", CompanyName = ""},
                new LeadPartRecord {Id = 63, Subject = "need .Net developer to complete the website", Description = "", CompanyName = ""},
                new LeadPartRecord {Id = 64, Subject = "PhoneGap App for Free Inner Oracle Cards", Description = "做一个纸牌类的游戏，", CompanyName = ""},
                new LeadPartRecord {Id = 65, Subject = "integrating N2CMS with your existing application", Description = "", CompanyName = ""},
                new LeadPartRecord {Id = 66, Subject = "Amazon Express Checkout plugin for NopCommerce 2.4", Description = "", CompanyName = "inspiredagency.co.uk"},
                new LeadPartRecord {Id = 67, Subject = "Tryon Function", Description = "", CompanyName = ""},
                new LeadPartRecord {Id = 68, Subject = "Legacy program rewritten", Description = "I had a pr", CompanyName = ""},
                new LeadPartRecord {Id = 69, Subject = "Year Make Model auto parts", Description = "", CompanyName = ""},
                new LeadPartRecord {Id = 70, Subject = " long term partnership and first project", Description = "", CompanyName = "The Small Axe"},
                new LeadPartRecord {Id = 71, Subject = "某产权交易中心交易流程管理系统", Description = "客户是一家软件开发公", CompanyName = "重庆莱基科技有限公司"},
                new LeadPartRecord {Id = 72, Subject = "Joomla Extension", Description = "", CompanyName = ""},
                new LeadPartRecord {Id = 73, Subject = "Opencart 1.5.5.1 default theme customization", Description = "", CompanyName = ""},
                new LeadPartRecord {Id = 74, Subject = "Sitefinity Reskin 3.7 CMS", Description = "Message: W", CompanyName = ""},
                new LeadPartRecord {Id = 75, Subject = "Site Cleaned up and Finish", Description = "Message: ", CompanyName = ""},
                new LeadPartRecord {Id = 76, Subject = "Looking for an ASP.NET developer", Description = "", CompanyName = ""},
                new LeadPartRecord {Id = 77, Subject = "Wordpress Page", Description = "", CompanyName = ""},
                new LeadPartRecord {Id = 78, Subject = "hanges on an OC extension", Description = "", CompanyName = ""},
                new LeadPartRecord {Id = 79, Subject = "Long Term Relationship ", Description = "Hi there, ", CompanyName = "TheBestAupair "},
                new LeadPartRecord {Id = 80, Subject = "HTML5 questionnaire ", Description = "", CompanyName = ""},
                new LeadPartRecord {Id = 81, Subject = "AX2012 and Nopcommerce integration ", Description = "", CompanyName = ""},
                new LeadPartRecord {Id = 82, Subject = "Uber-like System", Description = "", CompanyName = ""},
                new LeadPartRecord {Id = 83, Subject = "Webgage Modification on DNN", Description = "Message: ", CompanyName = ""},
                new LeadPartRecord {Id = 84, Subject = "an external URL ", Description = "", CompanyName = ""},
                new LeadPartRecord {Id = 85, Subject = "nopCommerce customization", Description = "", CompanyName = ""},
                new LeadPartRecord {Id = 86, Subject = "OpenCV", Description = "Message: I", CompanyName = ""},
                new LeadPartRecord {Id = 87, Subject = "developing a shopping cart like namecheap.com", Description = "", CompanyName = ""},
                new LeadPartRecord {Id = 88, Subject = "Year Make Model", Description = "", CompanyName = "个人"},
                new LeadPartRecord {Id = 89, Subject = "Please contact me", Description = "Message: P", CompanyName = ""},
                new LeadPartRecord {Id = 90, Subject = ".NET MVC 4.5 - JSON Bootstrap. ", Description = "", CompanyName = ""},
                new LeadPartRecord {Id = 91, Subject = "Theme implementation for the WordPress Site", Description = "", CompanyName = ""},
                new LeadPartRecord {Id = 92, Subject = "Holiday Portal with DNN", Description = "Message: H", CompanyName = ""},
                new LeadPartRecord {Id = 93, Subject = "Building up a Similar Site", Description = "Message: ", CompanyName = ""},
                new LeadPartRecord {Id = 94, Subject = "nopCommerce Project", Description = "", CompanyName = "ADC Software"},
                new LeadPartRecord {Id = 95, Subject = "Total Costs for a Joomla Website Production", Description = "Message: ", CompanyName = ""},
                new LeadPartRecord {Id = 96, Subject = "WordPress Plugin", Description = "", CompanyName = ""},
                new LeadPartRecord {Id = 97, Subject = "nopCommerce SSL", Description = "", CompanyName = ""},
                new LeadPartRecord {Id = 98, Subject = "卖场的化妆品的手机APP", Description = "打电话来的许先生在为", CompanyName = ""},
                new LeadPartRecord {Id = 99, Subject = "B2B Website", Description = "Message: ", CompanyName = ""},
                new LeadPartRecord {Id = 100, Subject = "Free Trial First", Description = "Message: p", CompanyName = ""}
            };

            return leads;
        }
    }
}