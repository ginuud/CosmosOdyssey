# Define base image for Node.js
FROM node:18-alpine AS builder

# Set working directory
WORKDIR /app

# Copy package.json and package-lock.json
COPY package*.json ./

# Install dependencies
RUN npm install

# Copy all source code
COPY . .

# Build the application (if needed)
# RUN npm run build

# Runtime
FROM node:18-alpine
WORKDIR /app

# Copy only necessary files from the builder stage
COPY --from=builder /app .

# Expose the port
EXPOSE 3000

# Add a non-root user
RUN addgroup -S runner && adduser -S runner -G runner
USER runner

# Start the application
CMD ["npm", "start"]