using System;
using System.Web.Mvc;
using Roadkill.Core.Mvc.Controllers;
using Roadkill.Core.Plugins;

namespace Roadkill.ExamplePlugins.SpecialPage
{
	public class SoundCloud : SpecialPagePlugin
	{
		public override string Name
		{
			get
			{
				return "SoundCloud";
			}
		}

		public override ActionResult GetResult(SpecialPagesController controller)
		{
			return new ViewResult() { ViewName = "SoundCloud/SoundCloud", };
		}
	}
}
