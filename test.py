from centroid import centroid

for env in ['Dev', 'Test', 'Prod']:
	config = centroid().environment(env)
	
	admin_user = config.Database.admin.user_name
	admin_password = config.Database.admin.password
	db = config.Database.Server

	print '%s => Server=%s;Username=%s;Password=%s' % (env, db, admin_user, admin_password)