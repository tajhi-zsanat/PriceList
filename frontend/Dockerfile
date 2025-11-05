# --- Build stage ---
FROM node:20-alpine AS build
WORKDIR /app
COPY frontend/package*.json ./
RUN npm ci

COPY frontend/ ./

# Bake API base URL (default to /api to use nginx proxy)
ARG VITE_API_BASE_URL=/api
ENV VITE_API_BASE_URL=$VITE_API_BASE_URL

RUN chmod +x node_modules/.bin/*   # <-- Fix permissions for all binaries
RUN npm run build
RUN npm prune --production

# --- Nginx stage ---
FROM nginx:alpine
# SPA routing + API reverse proxy
COPY frontend/nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/dist/ /usr/share/nginx/html/
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
