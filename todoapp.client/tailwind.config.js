import { heroui } from "@heroui/theme"

/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./index.html",
    './src/layouts/**/*.{js,ts,jsx,tsx,mdx}',
    './src/pages/**/*.{js,ts,jsx,tsx,mdx}',
    './src/components/**/*.{js,ts,jsx,tsx,mdx}',
    "./node_modules/@heroui/theme/dist/**/*.{js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {},
  },
  darkMode: "class",

  plugins: [heroui({
    themes: {
      dark: {
        colors: {
          // Primary and background colors from the first image
          primary: "#384B55", // blue from the first image
          // secondary: "#d3c6aa",
          // success: "#a7c080",
          // warning: "#e6985",
          // danger: "#e67e80",
          background: {
            DEFAULT: "#1E2326",  // bg_dim from first image
            50: "#272E33",       // bg0
            100: "#2E383C",      // bg1
            200: "#374145",      // bg2
            300: "#A7C080",      // statusline1
            400: "#DBB997",      // bg_visual
            500: "#3C4841",      // bg_green
            600: "#4C3743",      // bg_red
            700: "#384855",      // bg_blue
            800: "#454443",      // bg_yellow
            900: "#3C4841",      // current word
          },
          foreground: {
            DEFAULT: "#D3C6AA",  // fg
            50: "#A7C080",       // statusline1
          },
          // Detailed color mapping
          bg_dim: "#1E2326",
          bg0: "#272E33",
          bg1: "#2E383C",
          bg2: "#374145",
          bg3: "#414B50",
          bg4: "#495156",
          bg5: "#4F5B58",
          bg_statusline1: "#A7C080",
          bg_statusline2: "#D3C6AA",
          bg_statusline3: "#7A8478",
          fg0: "#D3C6AA",
          fg1: "#A7C080",

          // Colors from the first image
          red: "#E67E80",
          orange: "#E69875",
          yellow: "#DBBC7F",
          green: "#A7C080",
          blue: "#7FBBB3",
          aqua: "#83C092",
          purple: "#D699B6",

          // Background variants
          bg_red: "#4C3743",
          bg_green: "#3C4841",
          bg_yellow: "#454443",
          bg_blue: "#384855",

          // Grays
          grey0: "#7A8478",
          grey1: "#859289",
          grey2: "#9DA9A0",
        }
      },
      light: {
        colors: {
          // Primary and background colors from the second image
          primary: "#3A94C5", // blue from the second image
          // secondary: "#e0aaff",
          // success: "#606c38",
          // warning: "#ffd500",
          // danger: "#ef233c",
          background: {
            DEFAULT: "#F2EDDF",  // bg_dim from second image
            50: "#FFFBEF",       // bg0
            100: "#F8F5E4",      // bg1
            200: "#F2EDDF",      // bg2
            300: "#93B259",      // statusline1
            400: "#F0F2D4",      // bg_visual
            500: "#8DA101",      // bg_green
            600: "#F85552",      // bg_red
            700: "#ECF5ED",      // bg_blue
            800: "#DFAA00",      // bg_yellow
            900: "#8DA101",      // current word
          },
          foreground: {
            DEFAULT: "#5C6A72",  // fg
            50: "#93B259",       // statusline1
          },
          // Detailed color mapping
          bg_dim: "#F2EDDF",
          bg0: "#FFFBEF",
          bg1: "#F8F5E4",
          bg2: "#F2EDDF",
          bg3: "#EDEADA",
          bg4: "#E8E5D5",
          bg5: "#BEC5B2",
          bg_statusline1: "#93B259",
          bg_statusline2: "#5C6A72",
          bg_statusline3: "#7A8478",
          fg0: "#5C6A72",
          fg1: "#93B259",

          // Colors from the second image
          red: "#F85552",
          orange: "#F57D26",
          yellow: "#DFA000",
          green: "#8DA101",
          blue: "#3A94C5",
          aqua: "#35A77C",
          purple: "#DF69BA",

          // Background variants
          bg_red: "#F85552",
          bg_green: "#8DA101",
          bg_yellow: "#DFAA00",
          bg_blue: "#ECF5ED",

          // Grays
          grey0: "#5C6A72",
          grey1: "#939F91",
          grey2: "#829181",
        }
      }
    },
    prefix: "myapp",
  })],
}
