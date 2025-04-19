# Frontend Dockerfile
FROM node:20-bullseye AS builder

RUN npm install -g npm@latest && \
    apt-get update && \
    apt-get install -y python3 make g++

WORKDIR /app

COPY package.json .

RUN npm install --legacy-peer-deps && \
    npm install rollup@4.40.0 @rollup/rollup-linux-x64-gnu -D

COPY . .

RUN npm run build

FROM nginx:alpine
COPY --from=builder /app/.output/public /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]