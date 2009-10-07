
using System.Collections.Generic;

namespace Model
{
	public class Category
	{
		public Category()
		{
			Posts = new HashSet<Post>();
		}

		public virtual string Name { get; set; }

		public virtual int Id { get; set; }

		public virtual ICollection<Post> Posts { get; set; }
	}
}