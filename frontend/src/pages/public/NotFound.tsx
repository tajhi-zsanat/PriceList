import astronaut from "@/assets/img/NotFound/astronaut.svg";
import customLine from "@/assets/img/NotFound/custom-line.svg";
import rocketship from "@/assets/img/NotFound/rocketship.svg";
import satellite from "@/assets/img/NotFound/satellite.svg";
import starsBackground from "@/assets/img/NotFound/stars-background.gif";


import {
  motion,
  useMotionValue,
  useTransform,
  useAnimationFrame,
} from "framer-motion"
import { Link } from "react-router-dom";

// Animation constants
const ORBIT_RADIUS = 300
const ORBIT_SPEED = 0.0025 // radians per frame
const SATELLITE_DURATION = 40
const ASTRONAUT_DURATION = 10
const TEXT_PULSE_DURATION = 4


export default function NotFound() {
  // Angle motion value
  const angle = useMotionValue(0)
  // Update angle every frame
  useAnimationFrame(() => {
    angle.set(angle.get() + ORBIT_SPEED)
  })
  // Derived x/y coordinates
  const x = useTransform(angle, (a) => Math.cos(a) * ORBIT_RADIUS)
  const y = useTransform(angle, (a) => Math.sin(a) * ORBIT_RADIUS)
  const rocketRotation = useTransform(angle, (a) => (a * 180) / Math.PI + 90)

  return (
    <section className="relative w-full h-screen flex items-center justify-center text-white overflow-hidden">
      {/* Background */}
      <div className="absolute inset-0 -z-10">
        <img
          src={starsBackground}
          alt="Animated starry background"
          className="object-cover size-full"
        />
      </div>
      <div>
        <div>
          {/* Satellite */}
          <motion.div
            className="absolute top-1/4 left-/14"
            initial={{ x: -300, y: -100, rotate: 0 }}
            animate={{
              x: [-300, 300, -300],
              rotate: [0, 360],
            }}
            transition={{
              duration: SATELLITE_DURATION,
              repeat: Infinity,
              ease: "linear",
            }}
          >
            <img
              className="opacity-80"
              src={satellite}
              alt="Satellite"
              width={80}
              height={80}
            />
          </motion.div>
          {/* Astronaut */}
          <motion.div
            className="absolute top-[70%] left-[60%] opacity-80 -translate-x-1/2 -translate-y-1/2"
            initial={{ y: 0, rotate: 0 }}
            animate={{
              y: [0, -30, 0, 30, 0],
              rotate: [0, 5, -5, 0],
            }}
            transition={{
              duration: ASTRONAUT_DURATION,
              repeat: Infinity,
              ease: "easeInOut",
            }}
            style={{ top: "70%", left: "60%" }}
          >
            <img
              className="opacity-90 drop-shadow-[0_0_12px_rgba(255,255,255,0.5)] bg-black/20 rounded-full"
              src={astronaut}
              alt="Astronaut"
              width={100}
              height={100}
            />
          </motion.div>
          {/* Center Content */}
          <motion.div
            className="relative flex flex-col items-center text-center z-10 px-4"
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 1.2 }}
          >
            <motion.h1 className="font-noto text-5xl sm:text-7xl mb-4">
              چهار صدُ چهار
            </motion.h1>
            {/* Divider Line */}
            <img
              className="mt-6 sm:mt-10 mb-4 sm:mb-6 w-64 sm:w-96 md:w-[412px]"
              src={customLine}
              alt=""
              width={412}
              height={6}
            />
            {/* Text Content */}
            <motion.p
              className="text-lg sm:text-xl opacity-80 max-w-md px-4"
              animate={{ opacity: [0.8, 1, 0.8] }}
              transition={{ duration: TEXT_PULSE_DURATION, repeat: Infinity }}
            >
              مثل اینکه توی فضا گم شدی
            </motion.p>
            <motion.p
              className="mt-2 text-sm sm:text-md text-gray-300 px-4"
              animate={{ opacity: [1, 0.7, 1] }}
              transition={{ duration: TEXT_PULSE_DURATION, repeat: Infinity }}
            >

              صفحه ای که دنبالش بودی، افتاده توی یک سیاه چاله!
            </motion.p>
            <Link to="/" className="mt-8 sm:mt-12 group">
              <motion.div
                className="relative"
                initial={{ opacity: 0, y: 20 }}
                animate={{ opacity: 1, y: 0 }}
                transition={{ delay: 0.5, duration: 0.8 }}
                whileHover={{ scale: 1.05 }}
                whileTap={{ scale: 0.95 }}
              >
                {/* Flame/Thruster Effect */}
                <motion.div
                  className="absolute -bottom-8 left-1/2 -translate-x-1/2 w-12 h-12"
                  animate={{
                    opacity: [0.6, 1, 0.6],
                    scale: [0.8, 1.2, 0.8],
                  }}
                  transition={{
                    duration: 0.5,
                    repeat: Infinity,
                    ease: "easeInOut",
                  }}
                >
                  <div />
                  <div className="w-full h-full bg-gradient-to-b from-orange-500 via-yellow-400 to-transparent rounded-full blur-sm" />
                </motion.div>
                {/* Main Button - Capsule Shape */}
                <div className="relative bg-gradient-to-b from-gray-900 via-transparent to-gray-700 px-8 py-4 rounded-full border-2 border-gray-900 shadow-lg overflow-hidden">
                  {/* Metallic shine effect */}
                  <motion.div
                    className="absolute inset-0 bg-gradient-to-r from-transparent via-white to-transparent opacity-30"
                    animate={{
                      x: ["-100%", "200%"],
                    }}
                    transition={{
                      duration: 3,
                      repeat: Infinity,
                      ease: "linear",
                    }}
                  />
                  بازگشت به خانه
                </div>
                {/* Glow effect on hover */}
                <motion.div
                  className="absolute inset-0 rounded-full bg-blue-400/20 blur-xl -z-10"
                  initial={{ opacity: 0 }}
                  whileHover={{ opacity: 1 }}
                  transition={{ duration: 0.3 }}
                />
              </motion.div>
            </Link>
          </motion.div>
          {/* Rocket Orbiting 404 */}
          <motion.div
            className="absolute z-10 pointer-events-none"
            style={{
              x,
              y,
              rotate: rocketRotation,
            }}
          >
            <motion.div style={{ rotate: rocketRotation }}>
              <img
                src={rocketship}
                alt="Rocketship"
                width={90}
                height={90}
                className="drop-shadow-[0_0_8px_rgba(255,255,255,0.8)]"
              />
            </motion.div>
          </motion.div>
        </div>
      </div>
    </section>
  );
}
