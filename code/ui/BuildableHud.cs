using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using Warbase;

public class BuildableHud : Panel
{
	public Label Name;
	public Label Health;
	public Label Owner;
	public Panel Bar;
	public Panel FillBar;
	public TimeSince TimeSinceHover;
	public BuildableEntity ActiveEntity;

	public BuildableHud()
	{
		FillBar = Add.Panel( "fillbar" );
		Bar = Add.Panel( "healthbar" );
		Owner = Add.Label( "Owner", "ownerlabel" );
		Name = Add.Label( "Name", "namelabel" );
		Health = Add.Label( "100", "healthlabel" );
	}

	public override void Tick()
	{
		base.Tick();

		WarbasePlayer player = Local.Pawn as WarbasePlayer;
		if ( player == null ) return;

		var tr = player.EyeTrace;
		if (tr.Hit && tr.Entity.IsValid() && tr.Entity is BuildableEntity)
		{
			ActiveEntity = tr.Entity as BuildableEntity;
			TimeSinceHover = 0;
		}

		if ( ActiveEntity != null && ActiveEntity.IsValid() )
		{
			var health = ActiveEntity.Health;
			var healthMax = ActiveEntity.Item.MaxHealth;
			Name.Text = $"{ActiveEntity.Item.Name}";
			if ( ActiveEntity.IsOwnedByTeam() )
				Owner.Text = $"{ActiveEntity.GetOwnerTeam().Name}";
			else if ( ActiveEntity.IsOwnedByPlayer() )
				Owner.Text = $"{ActiveEntity.GetOwnerPlayer().Client?.Name ?? ActiveEntity.GetOwnerPlayer().Name}";
			else
				Owner.Text = "Unowned";


			if ( ActiveEntity.BuildableState == BuildableState.Blueprint || ActiveEntity.BuildableState == BuildableState.Building )
			{
				var fraction = ActiveEntity.Progress / ActiveEntity.Item.RequiredProgress;
				Health.Text = $"{Math.Round( fraction * 100 )}%";
				FillBar.Style.Width = Length.Fraction( fraction );
				Bar.SetClass( "notbuilt", true );
				FillBar.SetClass( "notbuilt", true );
			} else if ( ActiveEntity.BuildableState == BuildableState.Built )
			{
				Health.Text = $"{health.CeilToInt()} / {healthMax.CeilToInt()}";
				FillBar.Style.Width = Length.Fraction( health / healthMax );
				Bar.SetClass( "notbuilt", false );
				FillBar.SetClass( "notbuilt", false );
			}
		}

		SetClass( "active", TimeSinceHover < 0.2f );
	}
}
