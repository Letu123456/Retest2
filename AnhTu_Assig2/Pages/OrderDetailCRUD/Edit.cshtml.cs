﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AnhTu_Assig2.Context;
using AnhTu_Assig2.Models;

namespace AnhTu_Assig2.Pages.OrderDetailCRUD
{
    public class EditModel : PageModel
    {
        private readonly AnhTu_Assig2.Context.AppDBContext _context;

        public EditModel(AnhTu_Assig2.Context.AppDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public OrderDetail OrderDetail { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderdetail =  await _context.OrderDetails.FirstOrDefaultAsync(m => m.OrderId == id);
            if (orderdetail == null)
            {
                return NotFound();
            }
            OrderDetail = orderdetail;
           ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId");
           ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(OrderDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderDetailExists(OrderDetail.OrderId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool OrderDetailExists(int id)
        {
            return _context.OrderDetails.Any(e => e.OrderId == id);
        }
    }
}
