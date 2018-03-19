Ext.define('Ext.CommandLine',
	{
		extend: 'Ext.form.TextArea',
		alias: 'widget.cmdfield',
		cls: 'my-element',
		padding: 5,
		keyMapEnabled: true,

		initialize: function() {
			var me = this,
				conf = {
					keyMap: new Ext.util.KeyNav({
						target: me.id,
						enter: function (e) {
							var lines = e.target.value.split("\n");
							var line = lines[lines.length - 1];
							//this.setValue(this.getValue() + '\n');
							alert('command - "' + line + '"');
							return false;
						},
						scope: me
					})
				};

			Ext.apply(me, conf);
			me.callParent();
		}

	});