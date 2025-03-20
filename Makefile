dev-nginx:
	@docker kill nginxdev || true
	docker run -d --rm --name nginxdev -p 3000:80 -v ./nginx.conf:/etc/nginx/nginx.conf nginx
