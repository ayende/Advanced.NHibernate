using System;
using System.Collections.Generic;
using NHibernate.Search.Attributes;

namespace Model
{
	[Indexed]
	public class Post
	{
		public Post()
		{
			Tags = new HashSet<Tag>();
			Categories = new HashSet<Category>();
			Comments = new HashSet<Comment>();
		}

		[DocumentId]
		public virtual int Id { get; set; }

		[IndexedEmbedded]
		public virtual Blog Blog { get; set; }

		[IndexedEmbedded]
		public virtual User User { get; set; }

		[Field]
		public virtual string Title { get; set; }

		[Field]
		public virtual string Text { get; set; }

		[Field]
		public virtual DateTime PostedAt { get; set; }

		public virtual ICollection<Comment> Comments { get; set; }

		public virtual ICollection<Category> Categories { get; set; }

		public virtual ICollection<Tag> Tags { get; set; }
	}
}