/** @type {import('tailwindcss').Config} */
module.exports = {
    mode: "jit",
    content: [
        '../../node_modules/flyonui/dist/js/*.js',
        "./index.html",
        "./**/*.{fs,js,ts,jsx,tsx}",
    ],
    theme: {
        extend: {},
    },
    plugins: [
        require('flyonui'),
        require('flyonui/plugin')
    ]
}
