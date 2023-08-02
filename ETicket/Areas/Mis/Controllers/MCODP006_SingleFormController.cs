﻿using ETicket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebGrease.Css.Ast.Selectors;

namespace ETicket.Areas.Mis.Controllers
{
    /// <summary>
    /// MCODP006_SingleForm 單筆維護產生器
    /// </summary>
    public class MCODP006_SingleFormController : BaseController
    {
        [HttpGet]
        [LoginAuthorize()]
        public ActionResult Index()
        {
            PrgService.SetAction(enAction.Index, enCardSize.Large);
            PrgService.SetProgram();
            var model = new vmCodeModel();
            model.Id = 1;
            model.KeyColumn = "Id";
            model.AreaName = "Mis";
            model.ControllerName = "";
            model.ClassName = "";
            model.PrgNo = "";
            model.PrgName = "";
            model.TypeNo = "A";
            model.BlockCount = 1;
            model.LayoutName = "_LayoutAdmin";
            return View(model);
        }

        [HttpPost]
        [LoginAuthorize()]
        public ActionResult Index(vmCodeModel model)
        {
            if (!ModelState.IsValid) return View(model);
            using (CodeGenerator code = new CodeGenerator())
            {
                using (z_repoPrograms prg = new z_repoPrograms())
                {
                    string str_area_name = "";
                    string str_prg_name = "";
                    string str_controller_name = "";
                    var prgData = prg.repo.ReadSingle(m => m.PrgNo == model.PrgNo);
                    if (prgData != null)
                    {
                        str_prg_name = prgData.PrgName;
                        str_area_name = prgData.AreaName;
                        str_controller_name = prgData.ControllerName;
                    }
                    if (string.IsNullOrEmpty(str_area_name))
                    {
                        TempData["ErrorMessage"] = "Programs 資料表未輸入區域名稱 !!";
                        return View(model);
                    }
                    if (string.IsNullOrEmpty(str_controller_name))
                    {
                        TempData["ErrorMessage"] = "Programs 資料表未輸入控制器名稱 !!";
                        return View(model);
                    }

                    model.PrgName = str_prg_name;
                    model.AreaName = str_area_name;
                    model.ControllerName = str_controller_name;

                    //Model
                    vmGeneratorModel gen = new vmGeneratorModel();
                    gen.TypeNo = model.TypeNo;
                    gen.MetadataModel = code.GetMetadataModel(model);
                    gen.RepositoryModel = code.GetRepositoryModel(model);
                    gen.ControllerModel = code.GetControllerModel(model);

                    model.ViewName = "Index";
                    model.TemplateName = "Index";
                    gen.IndexViewModel = code.IndexViewModel(model);

                    if (model.BlockCount == 1)
                    {
                        model.ViewName = "CreateEdit";
                        model.TemplateName = "CreateEdit1";
                        gen.CreateEditViewModel = code.CreateEdit1ViewModel(model);
                    }
                    if (model.BlockCount == 2)
                    {
                        model.ViewName = "CreateEdit";
                        model.TemplateName = "CreateEdit2";
                        gen.CreateEditViewModel = code.CreateEditNViewModel(model, 2);
                    }
                    if (model.BlockCount == 3)
                    {
                        model.ViewName = "CreateEdit";
                        model.TemplateName = "CreateEdit3";
                        gen.CreateEditViewModel = code.CreateEditNViewModel(model, 3);
                    }

                    TempData["ResultModel"] = gen;
                    return RedirectToAction("Result");
                }
            }
        }

        [HttpGet]
        public ActionResult Result()
        {
            using (CodeGenerator code = new CodeGenerator())
            {
                vmGeneratorModel model = (vmGeneratorModel)TempData["ResultModel"];
                return View(model);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Result(vmGeneratorModel model)
        {
            using (CodeGenerator code = new CodeGenerator())
            {
                code.CreateMetadataFile(model.MetadataModel);
                code.CreateRepositoryFile(model.RepositoryModel);
                code.CreateControllerFile(model.ControllerModel);
                code.CreateViewFile(model.IndexViewModel);
                code.CreateViewFile(model.CreateEditViewModel);

                TempData["ErrorMessage"] = "檔案建立成功!!";
                return RedirectToAction("Index");
            }
        }
    }
}