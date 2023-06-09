﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P328Pustok.DAL;
using P328Pustok.Enums;
using P328Pustok.Models;
using P328Pustok.Services;
using P328Pustok.ViewModels;

namespace P328Pustok.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class OrderController : Controller
    {
        private readonly PustokContext _context;
        private readonly IEmailSender _emailSender;

        public OrderController(PustokContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }
        public IActionResult Index(int page=1)
        {
            var query = _context.Orders.Include(x => x.OrderItems).AsQueryable();
            var data = PaginatedList<Order>.Create(query, page, 8);

            return View(data);
        }


        public IActionResult Detail(int id)
        {
            Order order = _context.Orders.Include(x => x.OrderItems).ThenInclude(x => x.Book).FirstOrDefault(x => x.Id == id);

            if (order == null)
                return View("Error");

            return View(order);
        }

        public IActionResult Accept(int id)
        {
            Order order = _context.Orders.FirstOrDefault(x => x.Id == id);

            if (order == null)
                return View("Error");

            order.Status = OrderStatus.Accepted;
            _context.SaveChanges();

            _emailSender.Send(order.Email, "Your order accepted", "Yout order accepted!");

            return RedirectToAction("index");
        }

        public IActionResult Reject(int id)
        {
            Order order = _context.Orders.FirstOrDefault(x => x.Id == id);

            if (order == null)
                return View("Error");

            order.Status = OrderStatus.Rejected;
            _context.SaveChanges();
            _emailSender.Send(order.Email, "Your order rejected", "Yout order rejected!");


            return RedirectToAction("index");
        }
        //public IActionResult Test()
        //{
        //    var enums = Enum.GetValues(typeof(OrderStatus));

        //    Dictionary<byte,string> data  = new Dictionary<byte,string>();

        //    foreach (var item in enums)
        //    {
        //        data.Add((byte)item, item.ToString());
        //    }

        //    return Json(data);
        //}
    }
}
