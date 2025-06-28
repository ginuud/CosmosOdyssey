<template>
    <div class="session-timer">
        <div class="timer-display" :class="{ 'warning': isWarning }">
            <div class="timer-text">
                <span class="timer-label">Session expiration time</span>
                <span class="timer-countdown">{{ formattedTime }}</span>
            </div>
        </div>

        <div v-if="showWarning" class="modal-overlay" style="z-index: 2000;">
            <div class="warning-modal">
                <UIcon name="i-lucide-triangle-alert" class="warning-icon" />
                <h3>Time to check out!</h3>
                <p>Your session will expire in less than a minute.</p>
                <p> After the session expires routes will be refreshed and your unfinished reservation will be lost!</p>
                <button @click="dismissWarning" class="dismiss-btn">I understand</button>
            </div>
        </div>

        <div v-if="showExpiredModal" class="modal-overlay">
            <div class="expired-modal">
                <UIcon name="i-lucide-timer-reset" class="warning-icon" />
                <h3>Session Expired</h3>
                <p>Your session has expired. Refreshing routes with new pricelist.</p>
            </div>
        </div>
    </div>
</template>

<script>

import { usePriceListStore } from '@/stores/priceListStore';

export default {

    name: 'SessionTimer',
    props: {
        sessionDuration: {
            type: Number,
            default: 15 * 60000
        },
        warningThreshold: {
            type: Number,
            default: 60000
        },
        reservationModalOpen: {
            type: Boolean,
            default: false
        }
    },
    data() {

        return {
            timeLeft: this.sessionDuration,
            timer: null,
            showWarning: false,
            showExpiredModal: false,
            warningDismissed: false,
            sessionHandled: false
        }
    },
    mounted() {
        this.initTimer();
    },
    beforeUnmount() {
        this.clearTimer();
    },
    methods: {
        async initTimer() {
            const priceListStore = usePriceListStore();
            const latestPriceList = await priceListStore.getLatestPriceList();
            if (
                latestPriceList && latestPriceList.validUntil) {
                this.timeLeft = latestPriceList.validUntil.getTime() - Date.now();
                this.startTimer();
            } else {
                this.timeLeft = this.sessionDuration;
                this.startTimer();
            }
        },
        startTimer() {
            this.timer = setInterval(() => {
                this.timeLeft -= 1000;
            }, 1000);
        },

        clearTimer() {
            if (this.timer) {
                clearInterval(this.timer);
                this.timer = null;
            }
        },

        dismissWarning() {
            this.showWarning = false;
            this.warningDismissed = true;
        },

        async handleSessionExpiration() {
            if (this.sessionHandled) return;
            this.sessionHandled = true;
            this.clearTimer();
            this.showExpiredModal = true;

            this.$emit('session-expired', {
                isInReservationModule: this.reservationModalOpen
            });

            setTimeout(() => {
                this.showExpiredModal = false;
                this.handleRouteRefresh();
            }, 5000);
        },

        async handleRouteRefresh() {
            try {
                if (this.isInReservationModule()) {
                    this.$emit('close-reservation');
                }

                window.location.reload();

                this.showExpiredModal = false;

                this.resetTimer();

            } catch (error) {
                console.error('Error handling session expiration:', error);
            }
        },
        resetTimer() {
            this.timeLeft = this.sessionDuration;
            this.showWarning = false;
            this.showExpiredModal = false;
            this.warningDismissed = false;
            this.sessionHandled = false;
            this.startTimer();
        }
    },

    computed: {
        formattedTime() {
            const minutes = Math.floor(this.timeLeft / 60000);
            const seconds = Math.floor((this.timeLeft % 60000) / 1000);
            return `${minutes.toString().padStart(2, '0')}:${seconds.toString().padStart(2, '0')}`;
        },
        isWarning() {
            return this.timeLeft <= this.warningThreshold && this.timeLeft > 0;
        }
    },
    watch: {
        timeLeft(newTime) {
            if (newTime <= this.warningThreshold && newTime > 0 && !this.warningDismissed) {
                this.showWarning = true;
                this.$emit('show-warning');
            }
            if (newTime <= 0) {
                this.handleSessionExpiration();
            }
        }
    }

}
</script>

<style scoped>
.session-timer {
    position: relative;
    padding: 16px;
}

.timer-display {
    display: flex;
    align-items: center;
    gap: 8px;
    padding: 8px 16px;
    background: transparent;
    border: 1.5px solid #dee2e6;
    border-radius: 30px;
    font-family: monospace;
    transition: all 0.3s ease;
}

.timer-display.warning {
    background: #fff3cd;
    border-color: #ffc107;
    color: #856404;
}

.timer-display.warning .timer-label {
    color: #242423;
    animation: pulse 1.5s infinite;
}

.timer-text {
    display: flex;
    flex-direction: column;
    gap: 2px;
}

.timer-label {
    font-size: 15px;
    font-family: 'orbitron', sans-serif;
    color: #fafcfd;
}

.timer-countdown {
    font-size: 16px;
    font-weight: bold;
}

.modal-overlay {
    position: fixed;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background: rgba(0, 0, 0, 0.5);
    display: flex;
    align-items: center;
    justify-content: center;
    z-index: 3000;
}

.warning-modal,
.expired-modal {
    background: black;
    padding: 24px;
    border-radius: 12px;
    text-align: center;
    max-width: 600px;
    border: 2px solid #dee2e6;
}

.warning-icon,
.expired-icon {
    font-size: 48px;
    margin-bottom: 16px;
}

.warning-modal h3,
.expired-modal h3 {
    color: #ee1919;
    font-size: 20px;
}

.warning-modal p,
.expired-modal p {
    margin: 16px;
}

.dismiss-btn {
    background: #007bff;
    color: white;
    border: none;
    padding: 8px 16px;
    border-radius: 4px;
    cursor: pointer;
}

.dismiss-btn:hover {
    background: #0056b3;
}
</style>