# # --- Builder ---
# FROM node:20-alpine AS build

# # Define build arguments for environment variables
# ARG VITE_API_URL
# # Set environment variables during the build process
# ENV VITE_API_URL=${VITE_API_URL}


# WORKDIR /app

# # Better layer caching
# COPY package*.json ./
# RUN npm ci && npm cache clean --force

# COPY . .

# RUN npm run build

# # --- Runtime (Nginx) ---
# FROM nginx:alpine AS runtime
# COPY --from=build /app/dist /usr/share/nginx/html
# COPY nginx.conf /etc/nginx/nginx.conf
# EXPOSE 5273
# CMD ["nginx", "-g", "daemon off;"]


# --- Build stage ---
FROM node:20-alpine AS build
WORKDIR /app

# Install deps
COPY package*.json ./
RUN npm ci

# Build
COPY . .
RUN npm run build  # outputs to /app/dist

# --- Runtime stage (NGINX) ---
FROM nginx:alpine

# Copy site
COPY --from=build /app/dist/ /usr/share/nginx/html/

# NGINX site config: proxy /api -> todoapp-server:5291 and SPA fallback
# Put your file at ./nginx/default.conf (repo root has /nginx folder in your screenshot)
COPY ./nginx/default.conf /etc/nginx/conf.d/default.conf

# (Optional) If you keep a global nginx.conf, copy it instead of the default one:
# COPY ./nginx/nginx.conf /etc/nginx/nginx.conf

EXPOSE 80
