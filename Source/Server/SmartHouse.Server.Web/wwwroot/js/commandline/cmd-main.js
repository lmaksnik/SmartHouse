Ext.onReady(function() {
	Ext.create({
		xtype: 'viewport',
		renderTo: Ext.getBody(),
		layout: 'fit',
		items: [
			{
				xtype: 'cmdfield'
			}
		]
	});
})