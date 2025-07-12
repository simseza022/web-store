using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopCartApi.DataContext;
using ShopCartApi.Dtos;
using ShopCartApi.Models;

namespace ShopCartApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly DataContextEF _dataContextEF;
        IMapper _mapper;

        public PostsController(IConfiguration configuration) 
        {
            _dataContextEF = new DataContextEF(configuration);

            _mapper = new Mapper(
                new MapperConfiguration(cfg => {
                    cfg.CreateMap<PostToAdd, Post>();
                }));

        }


        [HttpGet("all")]
        public IActionResult Get()
        {
            if(_dataContextEF.Posts == null)
            {
                return Ok(Enumerable.Empty<Post>());
            }
            

            return Ok(_dataContextEF.Posts.ToList());
        }

        [HttpGet("{id}")]
        [Produces(typeof(Post))]
        public IActionResult GetSinglePost(int id)
        {
            List<Post>? posts = _dataContextEF.Posts.Where(p => p.Id == id).ToList();
            if (posts != null)
            {
                return Ok(posts);
            }


            throw new Exception("User not found");
        }
        [HttpGet("userPosts/{id}")]
        [Produces(typeof(Post))]
        public IActionResult GetUserPosts(int id)
        {
            List<Post>? posts = _dataContextEF.Posts.Where(p => p.UserId == id).ToList();
            if (posts != null)
            {
                return Ok(posts);
            }
            return Ok(Enumerable.Empty<Post>());
        }
        [HttpGet("myPosts")]
        [Produces(typeof(Post))]
        public IActionResult GetMYPosts()
        {
            int id = int.Parse(this.User.FindFirst("UserId")?.Value + "");
            List<Post>? posts = _dataContextEF.Posts.Where(p => p.UserId == id).ToList();
            if (posts != null)
            {
                return Ok(posts);
            }
            return Ok(Enumerable.Empty<Post>());
        }


        [HttpPost("add")]
        [Produces(typeof(Post))]
        public IActionResult AddUserPost(PostToAdd post)
        {

            string? userId = this.User.FindFirst("UserId")?.Value;
            if (userId != null)
            {
                Post newPost = _mapper.Map<Post>(post);
                newPost.UserId = int.Parse(userId);
                newPost.DateCreatedUtc = DateTime.UtcNow;
                _dataContextEF.Add<Post>(newPost);

                if(_dataContextEF.SaveChanges() > 0)
                {
                    return Ok();
                }
                return BadRequest("Error assing user post");
            }


            throw new Exception("User not found");
        }
        [HttpPut("edit")]
        [Produces(typeof(Post))]
        public IActionResult EditUserPost(EditPostDto editPost)
        {
            string? userId = this.User.FindFirst("UserId")?.Value;
            Post? dbPost = _dataContextEF.Posts?.Where(p => p.Id == editPost.Id).FirstOrDefault();
            if (dbPost != null)
            {
                if(userId == dbPost.UserId.ToString()) //The user that is editing the post , must be the one who created it
                {
                    dbPost.Title = editPost.Title;
                    dbPost.Description = editPost.Description;
                    dbPost.DateEditedUtc = DateTime.Now;

                    if (_dataContextEF.SaveChanges() > 0)
                    {
                        return Ok(dbPost);
                    }
                }
                
                throw new Exception("Error editing post");
            }


            throw new Exception("Post not found");
        }
        [HttpDelete("delete/{id}")]
        [Produces(typeof(IActionResult))]
        public IActionResult DeleteUserPost(int id)
        {
            string? userId = this.User.FindFirst("UserId")?.Value;

            Post? post = _dataContextEF.Posts?.Where(p=>p.Id == id).FirstOrDefault();
            if (post != null)
            {
                if (userId == post.UserId.ToString()) // The user deleting the post ,ust be the one who created it
                {

                    _dataContextEF.Remove(post);
                    if (_dataContextEF.SaveChanges() > 0)
                    {
                        return Ok();
                    }
                }
            }


            throw new Exception("Post not found");
        }

    }
}
