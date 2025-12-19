# .cursorrules for Interaktif Kredi Web Branch Project using .NET 8 Razor Pages
---
name: Interaktif Kredi .NET 8 Razor Pages Rules
globs:
  - "**/*.cs"
  - "**/*.cshtml"
  - "Styles/**/*.scss"
  - "**/*.js"
  - "**/*.ts"

rules:
  - description: ".NET & C# coding conventions for Interaktif Kredi"
    instructions: |
      - Use PascalCase for all C# properties and methods. Example: `public decimal LoanAmount { get; set; }`
      - Use decimal type exclusively for all financial data. Do NOT use float or double for any monetary values.
      - No complex C# logic inside .cshtml Razor views. All code logic must reside in corresponding PageModel (.cshtml.cs) classes.
      - Follow .NET 8 and C# 10+ idiomatic usage. Use pattern matching, null coalescing assignment, and implicit var typing where appropriate.

  - description: "Frontend HTML, SCSS, and jQuery coding rules"
    instructions: |
      - Use snake_case exclusively for CSS classes and IDs. For example: `.credit_amount_input`, `#submit_btn`.
      - Follow BEM methodology with snake_case for SCSS:
        - Block: .loan_card
        - Element: .loan_card__header
        - Modifier: .loan_card--active
      - Custom SCSS only: absolutely no CSS frameworks like Bootstrap or Tailwind allowed.
      - Never use !important in SCSS.
      - Use the given SCSS folders and architecture: Styles/abstracts, Styles/base, Styles/components, Styles/pages.
      - In JavaScript and jQuery code:
        - Cache jQuery selectors before using them more than once, e.g., `var $submit_btn = $('#submit_btn');`
        - Use snake_case for all JS variables and functions, e.g., `let total_interest = 0;` and `function calculate_payment() {}`.
        - Use event delegation syntax when binding events on dynamic elements.

  - description: "General project restrictions"
    instructions: |
      - Do not import or use any React or Next.js code from the WebSube-API folder; it is for visual reference only.
      - Avoid any use of kebab-case naming convention in front-end assets and code.
      - Make sure all image files use snake_case naming.
      - Ensure that all Razor Pages files use PascalCase naming matching project standards.

---

# Notes:
# Copy and add this `.cursorrules` file at the root of your project.
# The instructions in this file are automatically included in Cursor as Rules for AI,
# and override global settings in Cursor preferences.
# The more precise and specific your project rules, the better the AI assists you.

# Optional: If you'd like to split this single `.cursorrules` file into the newer `.cursor/rules` folder structure:
# 1. Create a folder `.cursor` in your project's root.
# 2. Inside `.cursor`, create a folder named `rules`.
# 3. Separate rules into multiple `.mdc` files such as:
#    - `instructions.mdc`: main global instructions with appropriate scope header.
#    - `csharp.mdc`: C#-specific coding rules.
#    - `scss.mdc`: SCSS/BEM and styling rules.
#    - `jquery.mdc`: jQuery/JS conventions.
# Remember to include scope headers in each `.mdc` file, e.g.:
# ```markdown
# ---
# name: SCSS and BEM rules
# globs:
#   - "Styles/**/*.scss"
# ---
# ```
# For complete guidance, see: https://www.instructa.ai/en/blog/how-to-use-cursor-rules-in-version-0-45
