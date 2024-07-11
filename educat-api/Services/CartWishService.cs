using Domain.DTOs.CartWishList;
using Domain.DTOs.Course;
using Domain.DTOs.User;
using Domain.Entities;
using educat_api.Context;
using Microsoft.EntityFrameworkCore;

namespace educat_api.Services
{
    public class CartWishService
    {
        private readonly AppDBContext _context;

        public CartWishService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CartItemOutDTO>> GetCartItemsByUserId(int userId)
        {
            try
            {
                var cartItems = await _context.CartWishList
                    .Where(c => c.Type == "cart" && c.FkUser == userId)
                    .Select(c => new CartItemOutDTO
                    {
                        PkCartWishList = c.PkCartWishList,
                        FkUser = c.FkUser,
                        FkCourse = c.FkCourse,
                        Type = c.Type,
                        Course = new CourseMinOutDTO
                        {
                            PkCourse = c.Course.PkCourse,
                            FkCategory = c.Course.FkCategory,
                            FKInstructor = c.Course.FKInstructor,
                            Active = c.Course.Active,
                            Summary = c.Course.Summary ?? "",
                            Title = c.Course.Title,
                            Price = c.Course.Price,
                            Cover = c.Course.Cover ?? "",
                            CretionDate = c.Course.CretionDate,
                            UpdateDate = c.Course.UpdateDate
                        },
                        Instructor = new UserMinOutDTO
                        {
                            PkUser = c.Course.Instructor.PkInstructor,
                            Name = c.Course.Instructor.User.Name,
                            LastName = c.Course.Instructor.User.LastName,
                            AvatarUrl = c.Course.Instructor.User.AvatarUrl
                        }
                    })
                    .ToListAsync();

                return cartItems;
            } catch (Exception)
            {
                throw;
            }
        }

        public async Task<CartWishOutDTO> CreateCartItem(int userId, int courseId)
        {
            try
            {
                bool findCourse = await _context.Courses
                    .AnyAsync(c => c.PkCourse == courseId && c.Active == true);

                if(!findCourse)
                {
                    throw new Exception("Curso no encontrado");
                }

                bool findcartItem = await _context.CartWishList
                    .AnyAsync(c => c.FkUser == userId && c.FkCourse == courseId && c.Type == "cart");

                if(findcartItem)
                {
                    throw new Exception("No se permite duplicados en el carrito");
                }


                CartWishList newCartItem = new()
                {
                    FkUser = userId,
                    FkCourse = courseId,
                    Type = "cart"
                };

                await _context.CartWishList.AddAsync(newCartItem);
                await _context.SaveChangesAsync();

                var cartItemDTO = new CartWishOutDTO
                {
                    PkCartWishList = newCartItem.PkCartWishList,
                    FkCourse = newCartItem.FkCourse,
                    FkUser = newCartItem.FkCourse,
                    Type = newCartItem.Type,
                    CreationDate = newCartItem.CreationDate
                };

                return cartItemDTO;
            } catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteCartItem(int userid, int cartItemId)
        {
            try
            {
                var cartItem = await _context.CartWishList
                    .FirstOrDefaultAsync(c => c.PkCartWishList == cartItemId && c.Type == "cart" && c.FkUser == userid);

                if (cartItem is null)
                {
                    throw new Exception("Item del carrito no encontrado");
                }

                _context.CartWishList.Remove(cartItem);
                await _context.SaveChangesAsync();
            } catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<CartItemOutDTO>> GetWishListByUserId(int userId)
        {
            try
            {
                var wishItems = await _context.CartWishList
                    .Where(c => c.Type == "wish" && c.FkUser == userId)
                    .Select(c => new CartItemOutDTO
                    {
                        PkCartWishList = c.PkCartWishList,
                        FkUser = c.FkUser,
                        FkCourse = c.FkCourse,
                        Type = c.Type,
                        Course = new CourseMinOutDTO
                        {
                            PkCourse = c.Course.PkCourse,
                            FkCategory = c.Course.FkCategory,
                            FKInstructor = c.Course.FKInstructor,
                            Active = c.Course.Active,
                            Summary = c.Course.Summary ?? "",
                            Title = c.Course.Title,
                            Price = c.Course.Price,
                            Cover = c.Course.Cover ?? "",
                            CretionDate = c.Course.CretionDate,
                            UpdateDate = c.Course.UpdateDate
                        },
                        Instructor = new UserMinOutDTO
                        {
                            PkUser = c.Course.Instructor.PkInstructor,
                            Name = c.Course.Instructor.User.Name,
                            LastName = c.Course.Instructor.User.LastName,
                            AvatarUrl = c.Course.Instructor.User.AvatarUrl
                        } 
                    })
                    .ToListAsync();

                return wishItems;
            } catch (Exception)
            {
                throw;
            }
        }
        public async Task<CartWishOutDTO> CreateWishListItem(int userId, int courseId)
        {
            try
            {
                bool findCourse = await _context.Courses
                    .AnyAsync(c => c.PkCourse == courseId && c.Active == true);

                if(!findCourse)
                {
                    throw new Exception("Curso no encontrado");
                }

                bool findWishItem = await _context.CartWishList
                    .AnyAsync(c => c.FkUser == userId && c.FkCourse == courseId && c.Type == "wish");

                if(findWishItem)
                {
                    throw new Exception("No se permite duplicados en la lista de deseos");
                }

                CartWishList newWishItem = new()
                {
                    FkUser = userId,
                    FkCourse = courseId,
                    Type = "wish"
                };

                await _context.CartWishList.AddAsync(newWishItem);
                await _context.SaveChangesAsync();

                var wishItemDTO = new CartWishOutDTO
                {
                    PkCartWishList = newWishItem.PkCartWishList,
                    FkCourse = newWishItem.FkCourse,
                    FkUser = newWishItem.FkCourse,
                    Type = newWishItem.Type,
                    CreationDate = newWishItem.CreationDate
                };

                return wishItemDTO;
            } catch (Exception)
            {
                throw;
            }
        }
        public async Task DeleteWishListItem(int userid, int courseId)
        {
            try
            {
                var wishItem = await _context.CartWishList
                    .FirstOrDefaultAsync(c => c.FkCourse == courseId && c.Type == "wish" && c.FkUser == userid);

                if (wishItem is null)
                {
                    throw new Exception("Item de la lista de deseos no encontrado");
                }
                
                _context.CartWishList.Remove(wishItem);
                await _context.SaveChangesAsync();
            } catch (Exception)
            {
                throw;
            }
        }
    }
}
